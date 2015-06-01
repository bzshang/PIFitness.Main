using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OSIsoft.AF.UnitsOfMeasure;
using PIFitness.Common;
using Fitbit.Models;

namespace PIFitness.Fitbit
{
    public class FitbitStreams : List<FitbitStream>
    {
        private IFitbitStreamFactory _streamFactory;

        private UOMs _uoms;

        public FitbitStreams(IFitbitStreamFactory streamFactory, UOMs uoms)
        {
            _streamFactory = streamFactory;
            _uoms = uoms;

            InitializeStreams();
        }

        private void InitializeStreams()
        {     

            this.Add(_streamFactory.CreateFitbitStream(TimeSeriesResourceType.CaloriesOut, "Calories", _uoms["calories (cal)"]));
            this.Add(_streamFactory.CreateFitbitStream(TimeSeriesResourceType.StepsTracker, "Steps", null));
            this.Add(_streamFactory.CreateFitbitStream(TimeSeriesResourceType.DistanceTracker, "Distance", _uoms["kilometer (km)"]));
            this.Add(_streamFactory.CreateFitbitStream(TimeSeriesResourceType.MinutesSedentaryTracker, "Minutes sedentary", _uoms["min"]));
            this.Add(_streamFactory.CreateFitbitStream(TimeSeriesResourceType.MinutesLightlyActiveTracker, "Minutes lightly active", _uoms["min"]));
            this.Add(_streamFactory.CreateFitbitStream(TimeSeriesResourceType.MinutesFairlyActiveTracker, "Minutes fairly active", _uoms["min"]));
            this.Add(_streamFactory.CreateFitbitStream(TimeSeriesResourceType.MinutesVeryActiveTracker, "Minutes very active", _uoms["min"]));
            this.Add(_streamFactory.CreateFitbitStream(TimeSeriesResourceType.MinutesAsleep, "Hours asleep", _uoms["min"]));
            this.Add(_streamFactory.CreateFitbitStream(TimeSeriesResourceType.MinutesAwake, "Minutes awake", _uoms["min"]));
            this.Add(_streamFactory.CreateFitbitStream(TimeSeriesResourceType.AwakeningsCount, "Awakenings count", null));
            this.Add(_streamFactory.CreateFitbitStream(TimeSeriesResourceType.TimeInBed, "Time spent in bed", _uoms["min"]));
            this.Add(_streamFactory.CreateFitbitStream(TimeSeriesResourceType.MinutesToFallAsleep, "Minutes to fall asleep", _uoms["min"]));
            this.Add(_streamFactory.CreateFitbitStream(TimeSeriesResourceType.MinutesAfterWakeup, "Minutes after wakeup", _uoms["min"]));
            this.Add(_streamFactory.CreateFitbitStream(TimeSeriesResourceType.TimeEnteredBed, "Time entered bed", null));
            this.Add(_streamFactory.CreateFitbitStream(TimeSeriesResourceType.SleepEfficiency, "Sleep efficiency", null));

        }


    }
}
