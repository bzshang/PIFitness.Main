using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject.Modules;

using PIFitness.AFSync;
using PIFitness.Domain.Interfaces;

namespace PIFitness.Factories.Modules
{
    public class UserSyncModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITableReader<UserEntry>>().To<UserDbReader>();
            Bind<ITableFilter<UserEntry>>().To<UserTableFilter>();

        }
    }
}
