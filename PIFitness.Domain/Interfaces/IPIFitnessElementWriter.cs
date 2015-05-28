using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF.Asset;

namespace PIFitness.Domain.Interfaces
{
    public interface IPIFitnessElementWriter
    {
        void CreateUserElement(string userName, string id, AFElementTemplate template);

        void CreateFitnessElement(string userName, string elementName, AFElementTemplate template);
    }
}
