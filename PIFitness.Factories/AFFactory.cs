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
        private static string gpxEfTemplate = ConfigurationManager.AppSettings["GPX Event Frame Template"];

        public static AFDatabase GetAFDatabase()
        {
            return new PISystems()[piSystem].Databases[afDatabase];
        }

        public static AFElementTemplate GetGPXEventFrameTemplate(AFDatabase database)
        {
            AFElementTemplate template = database.ElementTemplates[gpxEfTemplate];
            template.InstanceType = typeof(AFEventFrame);
            return template;
        }
    }
}
