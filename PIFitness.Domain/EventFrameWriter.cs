using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Ninject;

using PIFitness.Common.Interfaces;
using PIFitness.Log;

using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.Time;


namespace PIFitness.Common
{
    public class EventFrameWriter : IEventFrameWriter
    {
        private AFDatabase _db;

        public EventFrameWriter(AFDatabase db)
        {
            _db = db;
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

    }
}
