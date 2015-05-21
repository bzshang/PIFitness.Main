using System.Linq;

namespace PIFitness.GPX.Interfaces
{
    public interface IGPXTableFilter<T>
    {
        IQueryable<T> FilterTable(IQueryable<T> table);


    }
}
