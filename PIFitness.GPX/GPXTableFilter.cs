using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PIFitness.Domain.Interfaces;

namespace PIFitness.GPX
{
    public class GPXTableFilter : ITableFilter<GPXEntry>
    {

        public IQueryable<GPXEntry> FilterTable(IQueryable<GPXEntry> table)
        {
            var filteredTable = from rk in table
                        where rk.Processed == false &&
                              rk.FileName.Contains(@".gpx") &&
                              rk.Data != null &&
                              rk.UserId != null &&
                              rk.UserName != null
                        select rk;

            return filteredTable;
        }



    }
}
