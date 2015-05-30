using System.Linq;


namespace PIFitness.Common.Interfaces
{
    public interface ITableReader<T>
    {
        IQueryable<T> Read();
    }
}
