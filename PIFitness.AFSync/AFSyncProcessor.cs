﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using PIFitness.Log;
using PIFitness.Domain.Interfaces;

namespace PIFitness.AFSync
{
    public class AFSyncProcessor : IPIFitnessProcessor
    {
        private IPIFitnessTableReader<UserEntry> _reader;

        private IPIFitnessElementWriter _elementWriter;

        public AFSyncProcessor(IPIFitnessTableReader<UserEntry> reader, IPIFitnessElementWriter elementWriter)
        {
            _reader = reader;
            _elementWriter = elementWriter;
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
                    string userName = row.UserName;
                    string id = row.Id;
                    _elementWriter.CreateUserElementTree(userName, id);
                    PIFitnessLog.Write(TraceEventType.Information, 0, string.Format("Created AF element for user {0}", userName));
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