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
    public class GPXTableReader : ITableReader<GPXEntry>
    {
        private GPXRepository _gpxRepository;

        private ITableFilter<GPXEntry> _filter;

        public GPXTableReader(GPXRepository gpxRepository, ITableFilter<GPXEntry> filter)
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
