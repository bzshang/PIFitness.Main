using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF.Asset;

namespace PIFitness.Domain
{
    public class PIFitnessUser
    {
        private AFElement _element;
        private Lazy<GPXData> _gpxData;

        public PIFitnessUser(AFElement element)
        {
            _element = element;
        }

        public string Guid
        {
            get
            {
                return _element.Attributes["Guid"].GetValue().ToString();
            }
        }

        public string AccessToken
        {
            get
            {
                return _element.Attributes["Access Token"].GetValue().ToString();
            }
        }

        public string AccessTokenSecret
        {
            get
            {
                return _element.Attributes["Access Token Secret"].GetValue().ToString();
            }
        }

        public GPXData GPXData
        {
            get
            {
                return _gpxData.Value;
            }
        }

        public PIFitnessUser()
        {
            _gpxData = new Lazy<GPXData>(() =>
                                        {
                                            return new GPXData(this, _element.Elements["GPX"]);
                                        });

        }


    }
}
