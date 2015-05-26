using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFitness.GPX
{
    public class GPXRepository
    {
        private GPXDbContext gpxContext = new GPXDbContext();

        public IQueryable<GPXEntry> GpxTable
        {
            get { return gpxContext.RunKeepers; }
        }

        public void SaveSettings(GPXEntry gpxEntry)
        {
            GPXEntry dbEntry = gpxContext.RunKeepers.Find(gpxEntry.Id);
            if (dbEntry != null)
            {
                gpxContext.Entry(dbEntry).CurrentValues.SetValues(gpxEntry);
            }

            gpxContext.SaveChanges();

        }

    }
}
