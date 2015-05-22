using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PIFitness.Domain;
using PIFitness.Domain.Interfaces;

namespace PIFitness.AFSync
{
    public class UserDbReader : IPIFitnessTableReader<UserEntry>
    {
        private UserRepository _userRepository;

        private IPIFitnessTableFilter<UserEntry> _filter;

        public UserDbReader(UserRepository gpxRepository, IPIFitnessTableFilter<UserEntry> filter)
        {
            _userRepository = gpxRepository;
            _filter = filter;
        }

        public IQueryable<UserEntry> Read()
        {
            IQueryable<UserEntry> gpxTable = _userRepository.UserTable;

            gpxTable = _filter.FilterTable(gpxTable);

            return gpxTable;
        }


    }
}
