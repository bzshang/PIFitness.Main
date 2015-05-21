using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;

namespace PIFitness.Domain
{
    public class GPXData
    {
        public PIFitnessUser User { get; }

        private AFElement _element;

        public GPXData(PIFitnessUser user, AFElement element)
        {
            this.User = user;
            this._element = element;
        }

        public AFErrors<AFValue> UpdateValues(IList<AFValue> vals)
        {
            AFErrors<AFValue> result = AFListData.UpdateValues(vals, AFUpdateOption.Replace);

            return result;
        }

    }





}
