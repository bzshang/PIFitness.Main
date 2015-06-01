using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Time;
using OSIsoft.AF.UnitsOfMeasure;

using Fitbit.Models;


namespace PIFitness.Fitbit
{
    public class FitbitValuesConverter
    {


        public FitbitValuesConverter()
        {
        }

        public AFValues ConvertToAFValues(TimeSeriesDataList tsDataList, FitbitStream stream, FitbitUser fitbitUser)
        {

            AFValues values = new AFValues();
            foreach (var result in tsDataList.DataList)
            {
                AFValue val = null;
                if (stream.FitbitSource != TimeSeriesResourceType.TimeEnteredBed)
                {
                    val = new AFValue(Convert.ToSingle(result.Value), new AFTime(result.DateTime), stream.UnitsOfMeasure);
                }
                else
                {
                    val = new AFValue(result.Value, new AFTime(result.DateTime), stream.UnitsOfMeasure);
                }
                values.Add(val);

            }

            values.Attribute = fitbitUser.UserElement.Elements["Fitbit"].Attributes[stream.AttributeName];

            return values;
        }



    }
}
