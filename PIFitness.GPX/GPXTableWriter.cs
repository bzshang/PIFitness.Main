using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF.Asset;

using PIFitness.Entities;
using PIFitness.Common.Interfaces;

namespace PIFitness.GPX
{
    public class GPXTableWriter : ITableWriter<GPXEntry>
    {
        private GPXRepository _gpxRepository;

        public GPXTableWriter(GPXRepository gpxRepository)
        {
            _gpxRepository = gpxRepository;
        }

        public void UpdateRow(GPXEntry row)
        {
            _gpxRepository.SaveSettings(row);
            
        }


    }
}
