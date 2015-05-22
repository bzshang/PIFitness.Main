using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;

namespace PIFitness.Factories
{
    public static class AFFactory
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

        public static AFDatabase GetAFDatabase()
        {
            return new PISystems()[piSystem].Databases[afDatabase];
        }

        public static AFElementTemplate GetTemplate(AFDatabase database, TemplateType templateType)
        {
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
