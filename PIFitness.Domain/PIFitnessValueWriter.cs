﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;

using PIFitness.Domain.Interfaces;

namespace PIFitness.Domain
{
    public class PIFitnessValueWriter : IPIFitnessValueWriter
    {

        public PIFitnessValueWriter() { }

        public void UpdateValues(IList<AFValues> valsList)
        {
            IList<AFValue> valsToWrite = new List<AFValue>();

            valsToWrite = valsList.SelectMany(i => i).ToList();

            AFErrors<AFValue> errors = AFListData.UpdateValues(valsToWrite, AFUpdateOption.Replace);
        }

    }
}