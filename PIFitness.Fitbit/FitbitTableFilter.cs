using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PIFitness.Entities;
using PIFitness.Common.Interfaces;

namespace PIFitness.Fitbit
{
    public class FitbitTableFilter : ITableFilter<UserEntry>
    {

        public IQueryable<UserEntry> FilterTable(IQueryable<UserEntry> table)
        {
            var filteredTable = from user in table
                                where user.FitbitAuthToken != null &&
                                      user.FitbitAuthTokenSecret != null
                                select user;
                                
            return filteredTable;
        }



    }
}
