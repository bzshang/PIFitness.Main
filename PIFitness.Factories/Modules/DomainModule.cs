using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject.Activation;
using Ninject;
using Ninject.Modules;

using PIFitness.Domain;
using PIFitness.Domain.Interfaces;
using PIFitness.GPX;

using OSIsoft.AF;
using OSIsoft.AF.Asset;

namespace PIFitness.Factories.Modules
{
    public class DomainModule : NinjectModule
    {
        //private AFDatabase _db;

        //public DomainModule(AFDatabase db)
        //{
        //    _db = db;
        //}


        public override void Load()
        {
            Bind<IPIFitnessValueWriter>().To<PIFitnessValueWriter>();
            Bind<IList<IPIFitnessProcessor>>().To<List<IPIFitnessProcessor>>();
            Bind<AFElementLookup>().ToSelf().InSingletonScope();//.WithConstructorArgument("db", _db);
            Bind<AFDatabase>().ToMethod(context => AFFactory.GetAFDatabase()).InSingletonScope();
            Bind<AFElementTemplate>().ToMethod(context => 
                AFFactory.GetGPXEventFrameTemplate(context.Kernel.Get<AFDatabase>())).InSingletonScope();
        }


    }
}
