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
using PIFitness.Domain.Interfaces;
using PIFitness.GPX.Interfaces;

namespace PIFitness.GPX
{
    public class GPXProcessor : IFitnessProcessor
    {
        private ITableReader<GPXEntry> _reader;

        private IGPXRowProcessor _rowProcessor;

        private IValueWriter _valueWriter;

        private IElementWriter _elementWriter;

        private IGPXEFWriter _efWriter;

        private ITableWriter<GPXEntry> _writer;

        private AFElementTemplate _template;

        private const string FITNESS_ELEMENT_NAME = "GPX";

        public GPXProcessor(ITableReader<GPXEntry> reader, 
            IGPXRowProcessor rowProcessor,
            IValueWriter valueWriter,
            IGPXEFWriter efWriter,
            ITableWriter<GPXEntry> writer,
            IElementWriter elementWriter,
            [Named("GpxElement")] AFElementTemplate gpxTemplate)
        {
            _reader = reader;
            _rowProcessor = rowProcessor;
            _valueWriter = valueWriter;
            _efWriter = efWriter;
            _writer = writer;
            _template = gpxTemplate;
            _elementWriter = elementWriter;
        }

        public void Process()
        {
            var table = GetTable();
            if (table == null)
            {
                PIFitnessLog.Write(TraceEventType.Information, 0, "GPX table was null");
                return;
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
                        if (bUpdatedValues && bCreatedEF)
                        {
                            SetRowProcessed(row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    PIFitnessLog.Write(TraceEventType.Information, 0, ex);
                }
            }
            //});
            //_db.CheckIn(AFCheckedOutMode.ObjectsCheckedOutThisSession);
            sw.Stop();

            PIFitnessLog.Write(TraceEventType.Information, 0, string.Format("Time to process GPX: {0}", sw.Elapsed.ToString()));


        }

        private void CreateFitnessElement(string userName, string elementName, AFElementTemplate template)
        {
            _elementWriter.CreateFitnessElement(userName, elementName, template);
        }

        private IQueryable<GPXEntry> GetTable()
        {
            IQueryable<GPXEntry> result = _reader.Read();

            return result;
        }

        private RouteInfo ProcessRow(GPXEntry row)
        {
            PIFitnessLog.Write(TraceEventType.Information, 0, 
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
            return _valueWriter.UpdateValues(vals);
        }

        private bool CreateEventFrame(RouteInfo routeInfo)
        {
            return _efWriter.CreateEventFrame(routeInfo);
        }




    }
}
