using System.Linq;

namespace PIFitness.Common.Interfaces
{
    public interface ITableFilter<T>
    {
        IQueryable<T> FilterTable(IQueryable<T> table);


    }
}
