using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PIFitness.Common;
using PIFitness.Common.Interfaces;
using PIFitness.Log;
using System.Diagnostics;

namespace PIFitness.Entities
{
    public class UserDbReader : ITableReader<UserEntry>
    {
        private UserRepository _userRepository;

        private ITableFilter<UserEntry> _filter;

        public UserDbReader(UserRepository gpxRepository, ITableFilter<UserEntry> filter)
        {
            _userRepository = gpxRepository;
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
                PIFitnessLog.Write(TraceEventType.Error, 0, string.Format("Error in UserDbReader.Read(): {0}", ex.Message));
                return null;
            }
        }
    }
}
