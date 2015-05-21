using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

using OSIsoft.AF.Asset;
using OSIsoft.AF.Time;

using PIFitness.Log;
using PIFitness.Domain;
using PIFitness.GPX.Interfaces;

namespace PIFitness.GPX
{
    public class GPXRowProcessor : IGPXRowProcessor<GPXEntry>
    {

        private AFElementLookup _lookup;

        public GPXRowProcessor(AFElementLookup lookup)
        {
            _lookup = lookup;
        }

        public RouteInfo ProcessRow(GPXEntry row)
        {
            try
            {
                GpxType gpxObj = GetGpxType(row);

                string activityName = GetActivityName(gpxObj);

                List<TrackPoint> wayPoints = GetWayPoints(gpxObj);

                AFElement element = GetElementFromGuid(row.UserId);

                RouteInfo routeInfo = GetRouteInfo(element, wayPoints, activityName);

                return routeInfo;
            }
            catch (Exception ex)
            {
                PIFitnessLog.Write(TraceEventType.Information, 0, ex);
                return null;
            }
        }

        private GpxType GetGpxType(GPXEntry gpxEntry)
        {
            string gpxFile = gpxEntry.Data;

            //Ugly hack but use it for now to deal with 1.0 versions
            if (gpxFile.Contains(@"xmlns=""http://www.topografix.com/GPX/1/0"""))
            {
                gpxFile = gpxFile.Replace("<trk>", "<trk>\n\t<name>RunningFree</name>").Replace(@"http://www.topografix.com/GPX/1/0", "http://www.topografix.com/GPX/1/1");
                File.WriteAllText(@"C:\testRunningFree.gpx", gpxFile);
            }

            GpxType gpxActivity = null;
            XmlSerializer mySerial = new XmlSerializer(typeof(GpxType));
            using (TextReader reader = new StringReader(gpxFile))
            {
                try
                {
                    return gpxActivity = (GpxType)mySerial.Deserialize(reader);
                }
                catch (Exception)
                {
                    return null;
                }

            }

        }

        private string GetActivityName(GpxType gpxObj)
        {
            string activityName = "Route";
            try
            {
                if (gpxObj == null)
                {
                    PIFitnessLog.Write(TraceEventType.Information, 0, "GpxObj is null");
                    return activityName;
                }
            
                string temp = gpxObj.trk[0].name;

                // find split in string and separate into name and date of activity
                if (temp == null)
                {
                    PIFitnessLog.Write(TraceEventType.Information, 0, "activity name is null");
                    return activityName;
                }
                
                int i;
                for (i = 0; i < temp.Length; i++)
                {
                    if (char.IsDigit(temp[i]))
                    {
                        break;
                    }
                }

                string creator = gpxObj.creator;
                if (creator == null)
                {
                    PIFitnessLog.Write(TraceEventType.Information, 0, "creator is null");
                    return activityName;
                }
                
                if (creator.Contains("RunKeeper"))
                {
                    activityName = temp.Substring(0, --i);
                }
                else if(creator.Contains("Garmin Connect"))
                {
                    activityName = temp;
                }
                
                return activityName;
            }
            catch (Exception ex)
            {
                PIFitnessLog.Write(TraceEventType.Information, 0, ex);
                return "Route";
            }

        }

        private List<TrackPoint> GetWayPoints(GpxType gpxObj)
        {
            try
            {
                //pull all geopositional data into the structure
                List<TrackPoint> wayPoints = new List<TrackPoint>();
                foreach (trksegType trackSeg in gpxObj.trk[0].trkseg)
                {
                    foreach (wptType waypoint in trackSeg.trkpt)
                    {
                        wayPoints.Add(new TrackPoint(waypoint.time, (double)waypoint.lon, (double)waypoint.lat, (double)waypoint.ele));
                    }
                }
                return wayPoints;
            }
            catch (Exception ex)
            {
                PIFitnessLog.Write(TraceEventType.Information, 0, ex);
                return null;
            }

        }

        private AFElement GetElementFromGuid(string id)
        {
            AFElement element = _lookup.GetElementFromGuid(id);
            if (element != null)
            {
                return element;
            }
            else
            {
                PIFitnessLog.Write(TraceEventType.Information, 0, string.Format("Could not find AF element for user {0}", id));
                return null;
            }
        }

        private RouteInfo GetRouteInfo(AFElement element, List<TrackPoint> wayPoints, string activityName)
        { 
            if (element == null)
            {
                PIFitnessLog.Write(TraceEventType.Information, 0, "AF Element is null");
                return null;
            }
            try
            { 
                //update latitude, longitude, and elevation

                AFAttribute elevationAttribute = element.Elements["GPX"].Attributes["Elevation"];
                AFAttribute latitudeAttribute = element.Elements["GPX"].Attributes["Longitude"];
                AFAttribute longitudeAttribute = element.Elements["GPX"].Attributes["Latitude"];

                AFValues listElevationValues = new AFValues();
                AFValues listLatitudeValues = new AFValues();
                AFValues listLongitudeValues = new AFValues();

                foreach (TrackPoint point in wayPoints)
                {
                    AFTime timestamp = new AFTime(point.Time);
                    listElevationValues.Add(new AFValue(elevationAttribute, point.Elevation, timestamp));
                    listLatitudeValues.Add(new AFValue(latitudeAttribute, point.Latitude, timestamp));
                    listLongitudeValues.Add(new AFValue(longitudeAttribute, point.Longitude, timestamp));
                }

                //now update the activity tag

                AFAttribute rkActivityAttribute = element.Elements["GPX"].Attributes["Activity"];

                DateTime temp_time = (from pt in wayPoints
                                      orderby pt.Time ascending
                                      select pt.Time).FirstOrDefault();

                AFValues listActivityValues = new AFValues();
                listActivityValues.Add(new AFValue(rkActivityAttribute, activityName, temp_time));

                temp_time = (from pt in wayPoints
                             orderby pt.Time descending
                             select pt.Time).FirstOrDefault();

                temp_time = temp_time.AddSeconds((double)1.0); //increment by one second
                listActivityValues.Add(new AFValue(rkActivityAttribute, "Idle", temp_time));

                //package all the results

                IList<AFValues> listAFValues = new List<AFValues>();
                listAFValues.Add(listElevationValues);
                listAFValues.Add(listLatitudeValues);
                listAFValues.Add(listLongitudeValues);
                listAFValues.Add(listActivityValues);

                // get the EF info

                AFTime start = listLatitudeValues[0].Timestamp;
                AFTime end = listLatitudeValues[listLatitudeValues.Count - 1].Timestamp;

                string displayedTime = start.UtcTime.ToString();

                string efName = string.Format("{0} - {1} - {2}", element.Name, activityName, displayedTime);

                return new RouteInfo { Element = element,
                                       Values = listAFValues,
                                       ActivityName = activityName,
                                       UserName = element.Name,
                                       StartTime = start,
                                       EndTime = end };
            }
            catch (Exception ex)
            {
                PIFitnessLog.Write(TraceEventType.Information, 0, ex);
                return null;
            }


        }


    }
}
