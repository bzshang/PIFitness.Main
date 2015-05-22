using System.Linq;

namespace PIFitness.Domain.Interfaces
{
    public interface IPIFitnessTableFilter<T>
    {
        IQueryable<T> FilterTable(IQueryable<T> table);


    }
}
