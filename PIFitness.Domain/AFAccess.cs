using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using PIFitness.Log;
using PIFitness.Common.Interfaces;

using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Time;
using OSIsoft.AF.Data;
using OSIsoft.AF.EventFrame;

namespace PIFitness.Common
{
    public class AFAccess : IAFAccess
    {
        private AFDatabase _db;

        public AFAccess(AFDatabase db)
        {
            _db = db;
        }

        public AFElement GetElementFromGuid(string id)
        {
            AFElement element = null;

            element = _db.Elements.DefaultIfEmpty(null).FirstOrDefault(i => i.Attributes["Guid"].GetValue().ToString() == id);

            return element;
        }

        public void TryCreateUserElement(string userName, string id, AFElementTemplate template)
        {
            _db.Refresh();

            PIFitnessLog.Write(TraceEventType.Verbose, 0, string.Format("Checking if AF Element exists for user {0}", userName));
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


            AFElement fitnessElement = userElement.Elements[elementName];

            if (fitnessElement == null)
            {
                fitnessElement = userElement.Elements.Add(elementName, template);
                AFDataReference.CreateConfig(fitnessElement, false, null);

                _db.CheckIn(AFCheckedOutMode.ObjectsCheckedOutThisThread);
                PIFitnessLog.Write(TraceEventType.Information, 0, string.Format("Created fitness element {0} for user {1}", elementName, userName));
            }
        }

        public bool CheckEventFrameExists(AFElement element, string name, AFElementTemplate efTemplate)
        {
            AFNamedCollectionList<AFEventFrame> listEF = element.GetEventFrames(new AFTime("*"), 0, 1,
                AFEventFrameSearchMode.BackwardFromStartTime, name, null, efTemplate);

            if (listEF.Count > 0)
            {
                PIFitnessLog.Write(TraceEventType.Information, 0, string.Format("Event frame already exists: {0}", name));
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CreateEventFrame(string name, AFTime start, AFTime end, AFElement primaryReferencedElement, AFElementTemplate efTemplate)
        {
            try
            {
                AFEventFrame newEF = new AFEventFrame(_db, name, efTemplate);
                newEF.SetStartTime(start);
                newEF.SetEndTime(end);
                newEF.PrimaryReferencedElement = primaryReferencedElement;
                newEF.CheckIn();

                _db.CheckIn(AFCheckedOutMode.ObjectsCheckedOutThisThread);
                _db.Refresh();

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool UpdateValues(IList<AFValues> valsList)
        {
            IList<AFValue> valsToWrite = new List<AFValue>();

            valsToWrite = valsList.SelectMany(i => i).ToList();

            AFErrors<AFValue> errors = AFListData.UpdateValues(valsToWrite, AFUpdateOption.Replace);

            return (errors == null) ? true : false;
        }


    }
}
