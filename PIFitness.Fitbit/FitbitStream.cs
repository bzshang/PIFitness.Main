using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF.UnitsOfMeasure;

using Fitbit.Models;

namespace PIFitness.Fitbit
{
    public class FitbitStream
    {
        public TimeSeriesResourceType FitbitSource { get; set; }

        public string AttributeName { get; set; }

        public UOM UnitsOfMeasure { get; set; }

        public FitbitStream(TimeSeriesResourceType fitbitSource, string attributeName, UOM uom)
        {
            FitbitSource = fitbitSource;
            AttributeName = attributeName;
            UnitsOfMeasure = uom;
        }
    }
}
