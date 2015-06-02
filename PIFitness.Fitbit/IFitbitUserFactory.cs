using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fitbit.Api;
using OSIsoft.AF.Asset;

namespace PIFitness.Fitbit
{
    public interface IFitbitUserFactory
    {
        FitbitUser CreateFitbitUser(string consumerKey,
                                    string consumerSecret,
                                    string authToken,
                                    string authTokenSecret,
                                    AFElement element,
                                    bool isNew);
    }
}
