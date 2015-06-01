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
using Fitbit.Models;

namespace PIFitness.Fitbit
{
    public class FitbitProcessor : IFitnessProcessor
    {

        private IAFAccess _afAccess;

        private ITableReader<UserEntry> _reader;

        private IFitbitUserFactory _fitbitUserFactory;

        private Dictionary<string, FitbitUser> _fitbitUserCache;

        private const string FITNESS_ELEMENT_NAME = "Fitbit";

        private AFElementTemplate _template;

        private FitbitStreams _fitbitStreams;

        private FitbitValuesConverter _fitbitConverter;

        private readonly DateTime _startTime = DateTime.Now.AddDays(-30);
        private readonly DateTime _endTime = DateTime.Now;

        public FitbitProcessor(
            IAFAccess afAccess,
            ITableReader<UserEntry> reader, 
            IFitbitUserFactory fitbitUserFactory,
            Dictionary<string, FitbitUser> fitbitUserCache,
            FitbitStreams fitbitStreams,
            FitbitValuesConverter fitbitConverter,
            [Named("FitbitElement")] AFElementTemplate elementTemplate)
        {
            _afAccess = afAccess;
            _reader = reader;
            _fitbitUserFactory = fitbitUserFactory;
            _fitbitUserCache = fitbitUserCache;
            _template = elementTemplate;
            _fitbitStreams = fitbitStreams;
            _fitbitConverter = fitbitConverter;
        }

        public void Process()
        {
            var userEntries = _reader.Read();

            foreach (var userEntry in userEntries)
            {
                TryCreateFitnessElement(userEntry.UserName, FITNESS_ELEMENT_NAME, _template);

                FitbitUser fitbitUser = GetFitbitUser(userEntry);

                IList<AFValues> valsList = GetFitbitData(fitbitUser);

                _afAccess.UpdateValues(valsList);
            }


        }

        private void TryCreateFitnessElement(string userName, string elementName, AFElementTemplate template)
        {
            _afAccess.TryCreateFitnessElement(userName, elementName, template);
        }

        private IList<AFValues> GetFitbitData(FitbitUser fitbitUser)
        {
            IList<AFValues> valsList = new List<AFValues>();
            foreach (var stream in _fitbitStreams)
            {
                AFValues vals = GetFitbitDataForStream(stream, fitbitUser);
                valsList.Add(vals);

            }

            //get calculated attributes
            AFValues activeHours = GetActiveHours(valsList, fitbitUser);
            valsList.Add(activeHours);

            return valsList;

        }

        private AFValues GetFitbitDataForStream(FitbitStream stream, FitbitUser fitbitUser)
        {
            TimeSeriesDataList internalDataList = fitbitUser.ApiClient.GetTimeSeries(stream.FitbitSource, _startTime, _endTime);

            AFValues vals = _fitbitConverter.ConvertToAFValues(internalDataList, stream, fitbitUser);

            return vals;

        }

        private AFValues GetActiveHours(IList<AFValues> valsList, FitbitUser fitbitUser)
        {
            try
            {
                IEnumerable<AFValues> filteredValues = from vals in valsList
                                                       where vals != null
                                                       select vals;

                AFValues veryActiveValues = QueryAFValuesByName(filteredValues, "Minutes very active");
                AFValues fairlyActiveValues = QueryAFValuesByName(filteredValues, "Minutes fairly active");
                AFValues lightlyActiveValues = QueryAFValuesByName(filteredValues, "Minutes lightly active");

                var combined = from valsActive in veryActiveValues
                               join valsFairly in fairlyActiveValues on valsActive.Timestamp equals valsFairly.Timestamp
                               join valsLightly in lightlyActiveValues on valsFairly.Timestamp equals valsLightly.Timestamp
                               select new AFValue((Convert.ToSingle(valsActive.Value) +
                                   Convert.ToSingle(valsFairly.Value) +
                                   Convert.ToSingle(valsLightly.Value)) / 60,
                                   valsActive.Timestamp);

                AFValues values = new AFValues();
                foreach (var val in combined)
                {
                    values.Add(val);
                }

                values.Attribute = fitbitUser.UserElement.Elements["Fitbit"].Attributes["Active hours"];
                return values;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private AFValues QueryAFValuesByName(IEnumerable<AFValues> filteredValues, string attributeName)
        {
            AFValues valResult = (from vals in filteredValues
             where vals.Attribute.Name == attributeName
             select vals).FirstOrDefault();

            return valResult;
        }

        private FitbitUser GetFitbitUser(UserEntry userEntry)
        {
            string userName = userEntry.UserName;

            FitbitUser fitbitUser = null;
            _fitbitUserCache.TryGetValue(userName, out fitbitUser);

            if (fitbitUser == null)
            {
                fitbitUser = CreateFitbitUser(userEntry);
                
                if (fitbitUser == null)
                {
                    PIFitnessLog.Write(TraceEventType.Information, 0, string.Format("Unable to create FitbitUser for {0}", userName));
                    return null;
                }

                _fitbitUserCache[userName] = fitbitUser;
                PIFitnessLog.Write(TraceEventType.Information, 0, string.Format("Added {0} to FitbitClient cache", userName));
            }

            return fitbitUser;
        }

        private FitbitUser CreateFitbitUser(UserEntry userEntry)
        {
            AFElement userElement = _afAccess.GetElementFromName(userEntry.UserName);

            if (userElement == null) return null;

            return _fitbitUserFactory.CreateFitbitUser(ConfigurationManager.AppSettings["fitbitConsumerKey"],
                                                       ConfigurationManager.AppSettings["fitbitConsumerSecret"],
                                                       userEntry.FitbitAuthToken,
                                                       userEntry.FitbitAuthTokenSecret,
                                                       userElement);
        }

    }
}
