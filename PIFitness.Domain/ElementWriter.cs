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
using PIFitness.Common.Interfaces;

namespace PIFitness.Common
{
    public class ElementWriter : IElementWriter
    {
        private AFDatabase _db;

        public ElementWriter(AFDatabase db)
        {
            _db = db;
        }

        public void CreateUserElement(string userName, string id, AFElementTemplate template)
        {
            _db.Refresh();
            AFElement userElement = _db.Elements[userName];

            if (userElement == null)
            {
                userElement = _db.Elements.Add(userName, template);
                userElement.ExtendedProperties.Add("Guid", id);

                AFDataReference.CreateConfig(userElement, false, null);

                _db.CheckIn(AFCheckedOutMode.ObjectsCheckedOutThisThread);
                PIFitnessLog.Write(TraceEventType.Information, 0, string.Format("Created AF element for user {0}", userName));
            }
        }

        public void CreateFitnessElement(string userName, string elementName, AFElementTemplate template)
        {
            _db.Refresh();
            AFElement userElement = _db.Elements[userName];

            if (userElement == null)
            {
                PIFitnessLog.Write(TraceEventType.Information, 0, string.Format("Could not find AF element for user {0}", userName));
                return;
            }

            //lock(_db)
            //{
                AFElement fitnessElement = userElement.Elements[elementName];

                if (fitnessElement == null)
                {
                    fitnessElement = userElement.Elements.Add(elementName, template);
                    AFDataReference.CreateConfig(fitnessElement, false, null);

                    _db.CheckIn(AFCheckedOutMode.ObjectsCheckedOutThisThread);
                    PIFitnessLog.Write(TraceEventType.Information, 0, string.Format("Created fitness element {0} for user {1}", elementName, userName));
                }
            //}
        }

        public AFElement GetElementFromGuid(string id)
        {
            AFElement element = null;
            //lock (_db)
            //{
            element = _db.Elements.DefaultIfEmpty(null).FirstOrDefault(i => i.Attributes["Guid"].GetValue().ToString() == id);
            //}
            return element;
        }



    }
}
