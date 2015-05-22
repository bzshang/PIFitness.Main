using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFitness.Domain.Interfaces
{
    public interface IPIFitnessElementWriter
    {
        void CreateUserElementTree(string userName, string id);
    }
}
