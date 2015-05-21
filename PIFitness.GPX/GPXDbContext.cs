using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PIFitness.GPX
{
    public class GPXDbContext : DbContext
    {
        public DbSet<GPXEntry> RunKeepers { get; set; }

    }
}
