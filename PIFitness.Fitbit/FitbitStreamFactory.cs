using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fitbit.Models;

using OSIsoft.AF.UnitsOfMeasure;

namespace PIFitness.Fitbit
{
    public class FitbitStreamFactory : IFitbitStreamFactory
    {
        public FitbitStream CreateFitbitStream(TimeSeriesResourceType tsResourceType, string attributeName, UOM uom)
        {
            FitbitStream stream = new FitbitStream(tsResourceType, attributeName, uom);

            return stream;
        }
    }
}
