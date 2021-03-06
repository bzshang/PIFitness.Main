﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using OSIsoft.AF.Asset;

using PIFitness.Log;
using PIFitness.Entities;
using PIFitness.Common.Interfaces;


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
            try
            {
                IQueryable<GPXEntry> gpxTable = _gpxRepository.GpxTable;

                gpxTable = _filter.FilterTable(gpxTable);

                return gpxTable;
            }
            catch (Exception ex)
            {
                PIFitnessLog.Write(TraceEventType.Error, 0, string.Format("Error in GPXTableReader.Read(): {0}", ex.Message));
                return null;
            }
        }


    }
}
