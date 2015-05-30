using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF.Asset;

namespace PIFitness.Common.Interfaces
{
    public interface IElementWriter
    {
        void CreateUserElement(string userName, string id, AFElementTemplate template);

        void CreateFitnessElement(string userName, string elementName, AFElementTemplate template);

        AFElement GetElementFromGuid(string id);
    }
}
