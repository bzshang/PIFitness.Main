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
    public class CommonModule : NinjectModule
    {
        //private AFDatabase _db;

        //public DomainModule(AFDatabase db)
        //{
        //    _db = db;
        //}


        public override void Load()
        {
            Bind<IAFAccess>().To<AFAccess>().InSingletonScope();


            Bind<IValueWriter>().To<ValueWriter>().InSingletonScope(); 
            Bind<IElementWriter>().To<ElementWriter>().InSingletonScope();
            Bind<IEventFrameWriter>().To<EventFrameWriter>().InSingletonScope();

            Bind<ElementLookup>().ToSelf().InSingletonScope();

            Bind<AFDatabase>().ToMethod(context => AFFactory.GetAFDatabase()).InSingletonScope();

            Bind<AFElementTemplate>().ToMethod(context => 
                AFFactory.GetTemplate(context.Kernel.Get<AFDatabase>(),TemplateType.UserElement)).InSingletonScope().Named("UserElement");
            Bind<AFElementTemplate>().ToMethod(context =>
                AFFactory.GetTemplate(context.Kernel.Get<AFDatabase>(), TemplateType.FitbitElement)).InSingletonScope().Named("FitbitElement");
            Bind<AFElementTemplate>().ToMethod(context =>
                AFFactory.GetTemplate(context.Kernel.Get<AFDatabase>(), TemplateType.GPXElement)).InSingletonScope().Named("GpxElement");
            Bind<AFElementTemplate>().ToMethod(context =>
                AFFactory.GetTemplate(context.Kernel.Get<AFDatabase>(), TemplateType.GPXEventFrame)).InSingletonScope().Named("GpxEventFrame");
        }


    }
}
