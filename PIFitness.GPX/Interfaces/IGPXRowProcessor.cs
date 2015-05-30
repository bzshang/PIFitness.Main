using PIFitness.Entities;

namespace PIFitness.GPX.Interfaces
{
    public interface IGPXRowProcessor
    {
        RouteInfo ProcessRow(GPXEntry row);
        
    }
}
