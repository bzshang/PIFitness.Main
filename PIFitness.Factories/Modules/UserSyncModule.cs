using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject.Modules;

using PIFitness.Entities;
using PIFitness.UserSync;
using PIFitness.Common.Interfaces;

namespace PIFitness.Factories.Modules
{
    public class UserSyncModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITableReader<UserEntry>>().To<UserDbReader>().WhenInjectedInto(typeof(UserSyncProcessor));
            Bind<ITableFilter<UserEntry>>().To<UserTableFilter>().WhenInjectedInto(typeof(UserDbReader));

        }
    }
}
