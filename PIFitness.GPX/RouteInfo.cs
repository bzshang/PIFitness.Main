using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF.Asset;
using OSIsoft.AF.Time;

namespace PIFitness.GPX
{
    public class RouteInfo
    {
        public AFElement Element { get; set; }
        public string UserName { get; set; }
        public IList<AFValues> Values { get; set; }
        public string ActivityName { get; set; }
        public AFTime StartTime { get; set; }
        public AFTime EndTime { get; set; }
        public string UniqueName { get; set; }
    }
}
