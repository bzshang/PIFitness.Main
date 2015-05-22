using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Ninject;

using OSIsoft.AF;
using OSIsoft.AF.Asset;

using PIFitness.Log;
using PIFitness.Domain.Interfaces;

namespace PIFitness.Domain
{
    public class PIFitnessElementWriter : IPIFitnessElementWriter
    {
        private AFDatabase _db;

        private AFElementTemplate _userTemplate;

        private AFElementTemplate _gpxTemplate;

        private AFElementTemplate _fitbitTemplate;

        public PIFitnessElementWriter(AFDatabase db, 
            [Named("UserElement")] AFElementTemplate userTemplate,
            [Named("GpxElement")] AFElementTemplate gpxTemplate,
            [Named("FitbitElement")] AFElementTemplate fitbitTemplate) 
        {
            _db = db;
            _userTemplate = userTemplate;
            _gpxTemplate = gpxTemplate;
            _fitbitTemplate = fitbitTemplate;
        }

        public void CreateUserElementTree(string userName, string id)
        {
            _db.Refresh();
            AFElement userElement = _db.Elements[userName];

            if (userElement == null)
            {
                userElement = _db.Elements.Add(userName, _userTemplate);
                userElement.ExtendedProperties.Add("Guid", id);

                AFElement fitbitElement = userElement.Elements.Add("Fitbit", _fitbitTemplate);
                AFElement gpxElement = userElement.Elements.Add("GPX", _gpxTemplate);

                AFDataReference.CreateConfig(userElement, false, null);
                AFDataReference.CreateConfig(fitbitElement, false, null);
                AFDataReference.CreateConfig(gpxElement, false, null);

                _db.CheckIn(AFCheckedOutMode.ObjectsCheckedOutThisSession);
                PIFitnessLog.Write(TraceEventType.Information, 0, string.Format("Created AF element for user {0}", userName));
            }
        }



    }
}
