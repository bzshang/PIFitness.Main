using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF.Asset;

namespace PIFitness.Domain.Interfaces
{
    public interface IPIFitnessValueWriter
    {
        void UpdateValues(IList<AFValues> vals);
    }
}
