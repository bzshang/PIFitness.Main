using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ninject.Activation;
using Ninject;
using Ninject.Modules;

using PIFitness.Common;
using PIFitness.Common.Interfaces;
using PIFitness.GPX;
using PIFitness.Fitbit;
using PIFitness.UserSync;

using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;

namespace PIFitness.Factories.Modules
{
    public class CommonModule : NinjectModule
    {
        private AFFactory _afFactory;

        public CommonModule(AFFactory afFactory)
        {
            _afFactory = afFactory;
        }


        public override void Load()
        {
            Bind<IAFAccess>().To<AFAccess>().InSingletonScope();

            //Bind<IValueWriter>().To<ValueWriter>().InSingletonScope(); 
            //Bind<IElementWriter>().To<ElementWriter>().InSingletonScope();
            //Bind<IEventFrameWriter>().To<EventFrameWriter>().InSingletonScope();

            //Bind<ElementLookup>().ToSelf().InSingletonScope();

            Bind<AFDatabase>().ToMethod(context => _afFactory.GetAFDatabase()).InSingletonScope();
            Bind<UOMs>().ToMethod(context => _afFactory.GetUOMs()).InSingletonScope();

            Bind<AFElementTemplate>().ToMethod(context =>
                _afFactory.GetTemplate(context.Kernel.Get<AFDatabase>(), TemplateType.UserElement)).WhenInjectedInto(typeof(UserSyncProcessor)).InSingletonScope();//.Named("UserElement");
            Bind<AFElementTemplate>().ToMethod(context =>
                _afFactory.GetTemplate(context.Kernel.Get<AFDatabase>(), TemplateType.FitbitElement)).WhenInjectedInto(typeof(FitbitProcessor)).InSingletonScope();//.Named("FitbitElement");
            Bind<AFElementTemplate>().ToMethod(context =>
                _afFactory.GetTemplate(context.Kernel.Get<AFDatabase>(), TemplateType.GPXElement)).WhenInjectedInto(typeof(GPXProcessor)).Named("GpxElement");
            Bind<AFElementTemplate>().ToMethod(context =>
                _afFactory.GetTemplate(context.Kernel.Get<AFDatabase>(), TemplateType.GPXEventFrame)).WhenInjectedInto(typeof(GPXProcessor)).Named("GpxEventFrame");


        }


    }
}
