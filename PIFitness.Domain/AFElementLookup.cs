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

        private object _lockObject = new object();

        public AFElementLookup(AFDatabase db)
        {
            _db = db;
        }

        public AFElement GetElementFromGuid(string id)
        {
            AFElement element = null;
            lock (_lockObject)
            {
                element = _db.Elements.DefaultIfEmpty(null).FirstOrDefault(i => i.Attributes["Guid"].GetValue().ToString() == id);
            }
            return element;
        }




    }
}
