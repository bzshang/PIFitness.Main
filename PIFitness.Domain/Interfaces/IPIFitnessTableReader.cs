using System.Linq;


namespace PIFitness.Domain.Interfaces
{
    public interface IPIFitnessTableReader<T>
    {
        IQueryable<T> Read();
    }
}
