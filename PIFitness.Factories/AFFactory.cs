using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.UnitsOfMeasure;

namespace PIFitness.Factories
{

    /// <summary>
    /// Wrapper class enabling Ninject to construct AF objects on-the-fly via delegate in Binding
    /// </summary>
    public class AFFactory
    {

        private static string piSystem = ConfigurationManager.AppSettings["PISystem"];
        private static string afDatabase = ConfigurationManager.AppSettings["AFDatabase"];

        private static string userElementTemplate = ConfigurationManager.AppSettings["User Element Template"];
        private static string gpxElementTemplate = ConfigurationManager.AppSettings["GPX Element Template"];
        private static string fitbitElementTemplate = ConfigurationManager.AppSettings["Fitbit Element Template"];
        private static string gpxEfTemplate = ConfigurationManager.AppSettings["GPX Event Frame Template"];

        private static Dictionary<TemplateType, string> templateLookup = new Dictionary<TemplateType, string>()
        {
            { TemplateType.UserElement, userElementTemplate },
            { TemplateType.GPXElement, gpxElementTemplate },
            { TemplateType.FitbitElement, fitbitElementTemplate },
            { TemplateType.GPXEventFrame, gpxEfTemplate }
        };

        private Dictionary<string, object> _afFactoryCache;

        //Not used
        public AFFactory()
        {
            _afFactoryCache = new Dictionary<string, object>();
        }


        public AFDatabase GetAFDatabase()
        {
            //return ps.Databases[afDatabaseName];
            return new PISystems()[piSystem].Databases[afDatabase];
        }

        public UOMs GetUOMs()
        {
            return new PISystems()[piSystem].UOMDatabase.UOMs;

        }

        public AFElementTemplate GetTemplate(TemplateType templateType)
        {

            AFDatabase database = GetAFDatabase();
            AFElementTemplate template = database.ElementTemplates[templateLookup[templateType]];
            if (templateType == TemplateType.GPXEventFrame)
            {
                template.InstanceType = typeof(AFEventFrame);
            }
            else
            {
                template.InstanceType = typeof(AFElement);
            }
            return template;
        }

    }

    public enum TemplateType
    {
        UserElement,
        GPXElement,
        FitbitElement,
        GPXEventFrame
    }

}
