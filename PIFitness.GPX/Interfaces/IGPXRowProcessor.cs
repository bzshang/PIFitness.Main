namespace PIFitness.GPX.Interfaces
{
    public interface IGPXRowProcessor<T>
    {
        RouteInfo ProcessRow(T row);
        
    }
}
