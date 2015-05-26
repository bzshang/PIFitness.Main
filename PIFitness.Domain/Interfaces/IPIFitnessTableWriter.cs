using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFitness.Domain.Interfaces
{
    public interface IPIFitnessTableWriter<T>
    {
        void UpdateRow(T row);
    }
}
