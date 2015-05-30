using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;

using Ninject;

using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Time;

using PIFitness.Log;
using PIFitness.Entities;
using PIFitness.Common.Interfaces;

using Fitbit.Api;

namespace PIFitness.Fitbit
{
    public class FitbitProcessor : IFitnessProcessor
    {

        private IAFAccess _afAccess;

        private ITableReader<UserEntry> _reader;

        private IFitbitClientFactory _fitbitClientFactory;

        private Dictionary<string, FitbitClient> _fitbitClientCache;

        private const string FITNESS_ELEMENT_NAME = "Fitbit";

        private AFElementTemplate _template;

        public FitbitProcessor(IAFAccess afAccess,
            ITableReader<UserEntry> reader, 
            IFitbitClientFactory fitbitClientFactory,
            Dictionary<string, FitbitClient> fitbitClientCache,
            [Named("FitbitElement")] AFElementTemplate elementTemplate)
        {
            _afAccess = afAccess;
            _reader = reader;
            _fitbitClientFactory = fitbitClientFactory;
            _fitbitClientCache = fitbitClientCache;
            _template = elementTemplate;
        }

        public void Process()
        {
            var fitbitUsers = _reader.Read();

            foreach (var fitbitUser in fitbitUsers)
            {
                CreateFitnessElement(fitbitUser.UserName, FITNESS_ELEMENT_NAME, _template);

                UpdateCache(fitbitUser);
            }

            GetFitbitData(_fitbitClientCache);
             



        }

        private void GetFitbitData(IDictionary<string, FitbitClient> fitbitClientCache)
        {

            PIFitnessLog.Write(TraceEventType.Information, 0, "Finished getting Fitbit data");
        }

        private void CreateFitnessElement(string userName, string elementName, AFElementTemplate template)
        {
            _afAccess.CreateFitnessElement(userName, elementName, template);
        }

        private void UpdateCache(UserEntry fitbitUser)
        {

            string userName = fitbitUser.UserName;
            if (!_fitbitClientCache.ContainsKey(userName))
            {
                _fitbitClientCache[userName] = _fitbitClientFactory.CreateFitbitClient(
                                                        ConfigurationManager.AppSettings["fitbitConsumerKey"],
                                                        ConfigurationManager.AppSettings["fitbitConsumerSecret"],
                                                        fitbitUser.FitbitAuthToken,
                                                        fitbitUser.FitbitAuthTokenSecret);

                PIFitnessLog.Write(TraceEventType.Information, 0, string.Format("Added {0} to FitbitClient cache", userName));
            }

            
        }

    }
}
