using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fitbit.Api;

using OSIsoft.AF.Asset;

namespace PIFitness.Fitbit
{
    public class FitbitUser
    {
        public FitbitClient ApiClient { get; set; }

        public AFElement UserElement { get; set; }

        public FitbitUser(string consumerKey,
                          string consumerSecret,
                          string authToken,
                          string authTokenSecret,
                          AFElement element)
        {
            ApiClient = new FitbitClient(consumerKey, consumerSecret, authToken, authTokenSecret);
            UserElement = element;
        }


    }
}
