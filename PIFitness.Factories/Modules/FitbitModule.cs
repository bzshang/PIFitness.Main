using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PIFitness.Common.Interfaces;
using PIFitness.Entities;
using PIFitness.Fitbit;
using Ninject.Modules;
using Ninject.Extensions.Factory;

using Fitbit.Api;
using Fitbit.Models;

namespace PIFitness.Factories.Modules
{
    public class FitbitModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IFitbitUserFactory>().ToFactory();

            Bind<ITableFilter<UserEntry>>().To<FitbitTableFilter>().WhenInjectedInto(typeof(FitbitTableReader)); 
            Bind<ITableReader<UserEntry>>().To<FitbitTableReader>().WhenInjectedInto(typeof(FitbitProcessor));

            Bind<Dictionary<string, FitbitClient>>().ToSelf().InSingletonScope();

            Bind<FitbitStreams>().ToSelf().InSingletonScope();

            Bind<IFitbitStreamFactory>().To<FitbitStreamFactory>().InSingletonScope();
            //Bind<IFitbitStreamFactory>().ToFactory();
            Bind<TimeSeriesResourceType>().ToSelf();

            Bind<FitbitValuesConverter>().ToSelf().InSingletonScope();



        }
    }
}
