using System.Linq;


namespace PIFitness.GPX.Interfaces
{
    public interface IGPXTableReader<T>
    {
        IQueryable<T> Read();
    }
}
