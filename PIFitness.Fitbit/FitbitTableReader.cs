using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF.Asset;

using PIFitness.Entities;
using PIFitness.Common.Interfaces;
using PIFitness.Log;
using System.Diagnostics;

namespace PIFitness.Fitbit
{
    public class FitbitTableReader : ITableReader<UserEntry>
    {
        private UserRepository _userRepository;

        private ITableFilter<UserEntry> _filter;

        public FitbitTableReader(UserRepository userRepository, ITableFilter<UserEntry> filter)
        {
            _userRepository = userRepository;
            _filter = filter;
        }

        public IQueryable<UserEntry> Read()
        {
            try
            {
                IQueryable<UserEntry> userTable = _userRepository.UserTable;

                userTable = _filter.FilterTable(userTable);

                return userTable;
            }
            catch (Exception ex)
            {
                PIFitnessLog.Write(TraceEventType.Error, 0, string.Format("Error in FitbitTableReader.Read(): {0}", ex.Message));
                return null;
            }
        }


    }
}
