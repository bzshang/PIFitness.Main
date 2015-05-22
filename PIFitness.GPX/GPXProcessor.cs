using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using OSIsoft.AF.Asset;
using OSIsoft.AF.Time;

using PIFitness.Log;
using PIFitness.Domain.Interfaces;
using PIFitness.GPX.Interfaces;

namespace PIFitness.GPX
{
    public class GPXProcessor : IPIFitnessProcessor
    {
        private IPIFitnessTableReader<GPXEntry> _reader;

        private IGPXRowProcessor _rowProcessor;

        private IPIFitnessValueWriter _valueWriter;

        private IGPXEFWriter _efWriter;

        public GPXProcessor(IPIFitnessTableReader<GPXEntry> reader, 
            IGPXRowProcessor rowProcessor,
            IPIFitnessValueWriter valueWriter,
            IGPXEFWriter efWriter)
        {
            _reader = reader;
            _rowProcessor = rowProcessor;
            _valueWriter = valueWriter;
            _efWriter = efWriter;
        }

        public void Process()
        {
            var table = GetTable();
            if (table == null)
            {
                PIFitnessLog.Write(TraceEventType.Information, 0, "GPX table was null");
                return;
            }

            foreach (var row in table)
            {
                try
                {
                    RouteInfo routeInfo = ProcessRow(row);
                    if (routeInfo != null)
                    {
                        UpdateValues(routeInfo.Values);
                        CreateEventFrame(routeInfo);
                    }
                }
                catch (Exception ex)
                {
                    PIFitnessLog.Write(TraceEventType.Information, 0, ex);
                }
            }

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

        private void UpdateValues(IList<AFValues> vals)
        {
            _valueWriter.UpdateValues(vals);
        }

        private void CreateEventFrame(RouteInfo routeInfo)
        {
            _efWriter.CreateEventFrame(routeInfo);
        }




    }
}
