using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Ninject;

using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.Time;

using PIFitness.GPX.Interfaces;
using PIFitness.Log;

namespace PIFitness.GPX
{
    public class GPXEFWriter : IGPXEFWriter
    {
        private AFDatabase _db;
        private AFElementTemplate _efTemplate;

        public GPXEFWriter(AFDatabase db, [Named("GpxEventFrame")] AFElementTemplate efTemplate)
        {
            _db = db;
            _efTemplate = efTemplate;
        }

        public bool CreateEventFrame(RouteInfo routeInfo)
        {
            try
            { 
                AFElement element = routeInfo.Element;
                string name = routeInfo.UniqueName;
                AFTime start = routeInfo.StartTime;
                AFTime end = routeInfo.EndTime;

                //lock (_db)
                //{
                    AFNamedCollectionList<AFEventFrame> listEF = element.Elements["Routes"].GetEventFrames(new AFTime("*"), 0, 1, AFEventFrameSearchMode.BackwardFromStartTime, name, null, _efTemplate);

                    if (listEF.Count > 0)
                    {
                        PIFitnessLog.Write(TraceEventType.Verbose, 0, string.Format("Event frame already exists: {0}", name));
                        return true;
                    }

                    AFEventFrame newEF = new AFEventFrame(_db, name, _efTemplate);
                    newEF.SetStartTime(start);
                    newEF.SetEndTime(end);
                    newEF.PrimaryReferencedElement = element.Elements["Routes"];
                    newEF.CheckIn();

                    _db.CheckIn(AFCheckedOutMode.ObjectsCheckedOutThisThread);
                    _db.Refresh();
                //}

                return true;
            }
            catch
            {
                return false;
            }

        }

    }
}