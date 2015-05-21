using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF;
using OSIsoft.AF.Asset;

namespace PIFitness.Domain
{
    public class AFElementLookup
    {
        private AFDatabase _db;

        public AFElementLookup(AFDatabase db)
        {
            _db = db;
        }

        public AFElement GetElementFromGuid(string id)
        {
            return _db.Elements.DefaultIfEmpty(null).FirstOrDefault(i => i.Attributes["Guid"].GetValue().ToString() == id);
        }




    }
}
