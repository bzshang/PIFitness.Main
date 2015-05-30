﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF.Asset;

namespace PIFitness.Common.Interfaces
{
    public interface IValueWriter
    {
        bool UpdateValues(IList<AFValues> vals);
    }
}
