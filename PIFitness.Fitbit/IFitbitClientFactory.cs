using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fitbit.Api;

namespace PIFitness.Fitbit
{
    public interface IFitbitClientFactory
    {
        FitbitClient CreateFitbitClient(string consumerKey,
                                        string consumerSecret,
                                        string authToken,
                                        string authTokenSecret);
    }
}
