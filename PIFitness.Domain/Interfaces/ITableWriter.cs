using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFitness.Common.Interfaces
{
    public interface ITableWriter<T>
    {
        void UpdateRow(T row);
    }
}
