using System.Linq;

namespace PIFitness.Domain.Interfaces
{
    public interface ITableFilter<T>
    {
        IQueryable<T> FilterTable(IQueryable<T> table);


    }
}
