using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject.Modules;

using PIFitness.Domain;
using PIFitness.Domain.Interfaces;
using PIFitness.GPX;
using PIFitness.GPX.Interfaces;

namespace PIFitness.Factories.Modules
{
    public class GpxModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<IPIFitnessProcessor>().To<GPXProcessor>();
            Bind<IGPXRowProcessor>().To<GPXRowProcessor>();
            Bind<IPIFitnessTableFilter<GPXEntry>>().To<GPXTableFilter>();
            Bind<IPIFitnessTableReader<GPXEntry>>().To<GPXDbReader>();
            Bind<IGPXEFWriter>().To<GPXEFWriter>();




        }
    }
}
