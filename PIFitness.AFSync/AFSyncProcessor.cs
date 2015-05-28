﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Ninject;

using PIFitness.Log;
using PIFitness.Domain.Interfaces;

using OSIsoft.AF.Asset;

namespace PIFitness.AFSync
{
    public class AFSyncProcessor : IPIFitnessProcessor
    {
        private IPIFitnessTableReader<UserEntry> _reader;

        private IPIFitnessElementWriter _elementWriter;

        private AFElementTemplate _template;

        public AFSyncProcessor(IPIFitnessTableReader<UserEntry> reader, IPIFitnessElementWriter elementWriter,
            [Named("UserElement")] AFElementTemplate userTemplate)
        {
            _reader = reader;
            _elementWriter = elementWriter;
            _template = userTemplate;
        }


        public void Process()
        {
            var table = GetTable();
            if (table == null)
            {
                PIFitnessLog.Write(TraceEventType.Information, 0, "AspNetUsers table was null");
                return;
            }

            foreach (var row in table)
            {
                try
                {
                    PIFitnessLog.Write(TraceEventType.Verbose, 0, string.Format("Checking if AF Element exists for user {0}", row.UserName));
                    string userName = row.UserName;
                    string id = row.Id;
                    _elementWriter.CreateUserElement(userName, id, _template);
                }
                catch (Exception ex)
                {
                    PIFitnessLog.Write(TraceEventType.Information, 0, ex);
                }
            }


        }

        private IQueryable<UserEntry> GetTable()
        {
            IQueryable<UserEntry> result = _reader.Read();
            return result;
        }
    }
}
