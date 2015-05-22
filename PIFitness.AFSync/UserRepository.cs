using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFitness.AFSync
{
    public class UserRepository
    {
        private UserDbContext userContext = new UserDbContext();

        public IQueryable<UserEntry> UserTable
        {
            get { return userContext.UserSettingsTable; }
        }

        public void SaveSettings(UserEntry userEntry)
        {
            UserEntry dbEntry = userContext.UserSettingsTable.Find(userEntry.Id);
            if (dbEntry != null)
            {
                userContext.Entry(dbEntry).CurrentValues.SetValues(userEntry);
            }

            userContext.SaveChanges();

        }

    }
}
