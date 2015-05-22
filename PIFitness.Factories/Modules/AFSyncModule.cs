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
    public class AFSyncModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPIFitnessTableReader<UserEntry>>().To<UserDbReader>();
            Bind<IPIFitnessTableFilter<UserEntry>>().To<UserTableFilter>();

        }
    }
}
