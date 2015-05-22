using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PIFitness.Domain.Interfaces;

namespace PIFitness.AFSync
{
    public class UserTableFilter : IPIFitnessTableFilter<UserEntry>
    {

        public IQueryable<UserEntry> FilterTable(IQueryable<UserEntry> table)
        {
            var filteredTable = from user in table
                                select user;

            return filteredTable;
        }



    }
}
