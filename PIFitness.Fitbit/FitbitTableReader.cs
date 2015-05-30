using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF.Asset;

using PIFitness.Entities;
using PIFitness.Common.Interfaces;

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
            IQueryable<UserEntry> userTable = _userRepository.UserTable;

            userTable = _filter.FilterTable(userTable);

            return userTable;
        }


    }
}
