using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF.Asset;
using PIFitness.Domain;
using PIFitness.Domain.Interfaces;

namespace PIFitness.GPX
{
    public class GPXTableWriter : ITableWriter<GPXEntry>
    {
        private GPXRepository _gpxRepository;

        private object _lockObject = new object();

        public GPXTableWriter(GPXRepository gpxRepository)
        {
            _gpxRepository = gpxRepository;
        }

        public void UpdateRow(GPXEntry row)
        {
            //lock (_lockObject)
            //{
                _gpxRepository.SaveSettings(row);
            //}
        }


    }
}
