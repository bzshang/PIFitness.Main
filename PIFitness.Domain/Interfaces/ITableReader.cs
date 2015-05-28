using System.Linq;


namespace PIFitness.Domain.Interfaces
{
    public interface ITableReader<T>
    {
        IQueryable<T> Read();
    }
}
