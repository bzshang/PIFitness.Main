using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PIFitness.AFSync
{
    public class UserDbContext : DbContext
    {
        public DbSet<UserEntry> UserSettingsTable { get; set; }

        public UserDbContext()
        { }

    }
}
