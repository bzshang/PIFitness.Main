using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF.Asset;
using PIFitness.Domain;
using PIFitness.GPX.Interfaces;

namespace PIFitness.GPX
{
    public class GPXDbReader : IGPXTableReader<GPXEntry>
    {
        private GPXRepository _gpxRepository;

        private IGPXTableFilter<GPXEntry> _filter;

        public GPXDbReader(GPXRepository gpxRepository, IGPXTableFilter<GPXEntry> filter)
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
