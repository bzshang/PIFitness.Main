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
    public class GPXDbReader : IPIFitnessTableReader<GPXEntry>
    {
        private GPXRepository _gpxRepository;

        private IPIFitnessTableFilter<GPXEntry> _filter;

        public GPXDbReader(GPXRepository gpxRepository, IPIFitnessTableFilter<GPXEntry> filter)
        {
            _gpxRepository = gpxRepository;
            _filter = filter;
        }

        public IQueryable<GPXEntry> Read()
        {
            IQueryable<GPXEntry> gpxTable = _gpxRepository.GpxTable;

            gpxTable = _filter.FilterTable(gpxTable);

            return gpxTable;
        }


    }
}
