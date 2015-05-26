using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using OSIsoft.AF;
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

        private IPIFitnessTableWriter<GPXEntry> _writer;

        public GPXProcessor(IPIFitnessTableReader<GPXEntry> reader, 
            IGPXRowProcessor rowProcessor,
            IPIFitnessValueWriter valueWriter,
            IGPXEFWriter efWriter,
            IPIFitnessTableWriter<GPXEntry> writer)
        {
            _reader = reader;
            _rowProcessor = rowProcessor;
            _valueWriter = valueWriter;
            _efWriter = efWriter;
            _writer = writer;
        }

        public void Process()
        {
            var table = GetTable();
            if (table == null)
            {
                PIFitnessLog.Write(TraceEventType.Information, 0, "GPX table was null");
                return;
            }

            //_db.Refresh();
            //Parallel.ForEach<GPXEntry>(table, new ParallelOptions { MaxDegreeOfParallelism = 4 }, row =>
            foreach (var row in table)
            {
                try
                {
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
