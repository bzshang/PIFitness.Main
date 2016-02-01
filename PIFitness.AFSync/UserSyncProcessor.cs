using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Ninject;

using PIFitness.Log;
using PIFitness.Common.Interfaces;
using PIFitness.Entities;

using OSIsoft.AF.Asset;

namespace PIFitness.UserSync
{
    public class UserSyncProcessor : IFitnessProcessor
    {
        private ITableReader<UserEntry> _reader;

        private IAFAccess _afAccess;

        private AFElementTemplate _template;

        public UserSyncProcessor(ITableReader<UserEntry> reader, 
            IAFAccess afAccess,
            AFElementTemplate userTemplate)
        {
            _reader = reader;
            _afAccess = afAccess;
            _template = userTemplate;
        }


        public void Process()
        {
            var table = GetTable();
            if (table == null)
            {
                PIFitnessLog.Write(TraceEventType.Warning, 0, "AspNetUsers table was null");
                return;
            }

            foreach (var row in table)
            {
                try
                {
                    PIFitnessLog.Write(TraceEventType.Verbose, 0, string.Format("Checking if AF Element exists for user {0}", row.UserName));
                    string userName = row.UserName;
                    string id = row.Id;
                    _afAccess.TryCreateUserElement(userName, id, _template);
                }
                catch (Exception ex)
                {
                    PIFitnessLog.Write(TraceEventType.Error, 0, ex);
                }
            }


        }

        private IQueryable<UserEntry> GetTable()
        {
            PIFitnessLog.Write(TraceEventType.Verbose, 0, "Entering UserSyncProcessor.GetTable");


            IQueryable<UserEntry> result = _reader.Read();
            return result;
        }
    }
}
