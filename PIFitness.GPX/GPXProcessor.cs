using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Ninject;

using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Time;

using PIFitness.Log;
using PIFitness.Common.Interfaces;
using PIFitness.GPX.Interfaces;
using PIFitness.Entities;

namespace PIFitness.GPX
{
    public class GPXProcessor : IFitnessProcessor
    {
        private ITableReader<GPXEntry> _reader;

        private IGPXRowProcessor _rowProcessor;

        private IAFAccess _afAccess;

        private ITableWriter<GPXEntry> _writer;

        private AFElementTemplate _template;

        private AFElementTemplate _efTemplate;

        private const string FITNESS_ELEMENT_NAME = "Routes";

        public GPXProcessor(ITableReader<GPXEntry> reader, 
            IGPXRowProcessor rowProcessor,
            IAFAccess afAccess,
            ITableWriter<GPXEntry> writer,
            [Named("GpxElement")] AFElementTemplate gpxTemplate,
            [Named("GpxEventFrame")] AFElementTemplate efTemplate)
        {
            _reader = reader;
            _rowProcessor = rowProcessor;
            _afAccess = afAccess;
            _writer = writer;
            _template = gpxTemplate;
            _efTemplate = efTemplate;
        }

        public void Process()
        {
            IQueryable<GPXEntry> table = null;
            try
            {
                table = GetTable();
                if (table == null)
                {
                    PIFitnessLog.Write(TraceEventType.Warning, 0, "GPX table was null");
                    return;
                }
            }
            catch (Exception ex)
            {
                PIFitnessLog.Write(TraceEventType.Error, 0, ex);
            }
            
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //Parallel.ForEach<GPXEntry>(table, new ParallelOptions { MaxDegreeOfParallelism = 4 }, row =>
            foreach (var row in table)
            {
                try
                {
                    CreateFitnessElement(row.UserName, FITNESS_ELEMENT_NAME, _template);

                    RouteInfo routeInfo = ProcessRow(row);

                    if (routeInfo != null)
                    {
                        bool bUpdatedValues = UpdateValues(routeInfo.Values);
                        bool bCreatedEF = CreateEventFrame(routeInfo);
                        if (bUpdatedValues && bCreatedEF) SetRowProcessed(row);
                    }

                }
                catch (Exception ex)
                {
                    PIFitnessLog.Write(TraceEventType.Error, 0, ex);
                }
            }
            //});
            //_db.CheckIn(AFCheckedOutMode.ObjectsCheckedOutThisSession);
            sw.Stop();

            PIFitnessLog.Write(TraceEventType.Information, 0, string.Format("Time to process GPX: {0}", sw.Elapsed.ToString()));


        }

        private void CreateFitnessElement(string userName, string elementName, AFElementTemplate template)
        {
            _afAccess.TryCreateFitnessElement(userName, elementName, template);
        }

        private IQueryable<GPXEntry> GetTable()
        {
            IQueryable<GPXEntry> result = _reader.Read();

            return result;
        }

        private RouteInfo ProcessRow(GPXEntry row)
        {
            PIFitnessLog.Write(TraceEventType.Verbose, 0, 
                string.Format("Processing GPX for user {0}", row.UserName));
            
            RouteInfo tableRowInfo = _rowProcessor.ProcessRow(row);

            return tableRowInfo;
        }

        private void SetRowProcessed(GPXEntry row)
        {
            row.Processed = true;
            _writer.UpdateRow(row);
        }

        private bool UpdateValues(IList<AFValues> vals)
        {
            return _afAccess.UpdateValues(vals);
        }

        private bool CreateEventFrame(RouteInfo routeInfo)
        {
            AFElement element = routeInfo.Element.Elements["Routes"];
            bool efExists = _afAccess.CheckEventFrameExists(element, routeInfo.UniqueName, _efTemplate);

            bool isCreated = false;
            if (!efExists)
            {
                isCreated = _afAccess.CreateEventFrame(routeInfo.UniqueName, 
                    routeInfo.StartTime, 
                    routeInfo.EndTime,
                    element, _efTemplate);
            }

            return isCreated;

        }




    }
}
