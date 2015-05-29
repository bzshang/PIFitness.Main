using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF.Asset;
using OSIsoft.AF.Time;

namespace PIFitness.Domain.Interfaces
{
    public interface IEventFrameWriter
    {
        bool CreateEventFrame(string name, AFTime start, AFTime end, AFElement primaryReferencedElement, AFElementTemplate efTemplate);

        bool CheckEventFrameExists(AFElement element, string name, AFElementTemplate efTemplate);
    }
}
