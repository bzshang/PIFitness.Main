using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF.Asset;
using OSIsoft.AF.Time;

namespace PIFitness.Common.Interfaces
{
    public interface IAFAccess
    {
        void TryCreateUserElement(string userName, string id, AFElementTemplate template);

        void CreateFitnessElement(string userName, string elementName, AFElementTemplate template);

        AFElement GetElementFromGuid(string id);

        bool CreateEventFrame(string name, AFTime start, AFTime end, AFElement primaryReferencedElement, AFElementTemplate efTemplate);

        bool CheckEventFrameExists(AFElement element, string name, AFElementTemplate efTemplate);

        bool UpdateValues(IList<AFValues> vals);

    }
}
