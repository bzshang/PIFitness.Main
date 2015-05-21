using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using PIFitness.Log;
using PIFitness.Domain;
using PIFitness.Domain.Interfaces;
using PIFitness.GPX;
using PIFitness.Factories.Modules;

using OSIsoft.AF;

using Ninject;

namespace PIFitness.Service
{
    public partial class PIFitnessService : ServiceBase
    {
        private System.Timers.Timer _timer;
        private ServiceWorker _serviceWorker;

        private double pollingInterval;
        private double pollingOffset;
        private DateTime projected;

        public PIFitnessService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            PIFitnessLog.ConfigureLogging();
            PIFitnessLog.Write(TraceEventType.Information, 0, "PI Fitness Service is starting");


            //GPXRepository gpxRepository = new GPXRepository();
            //IPIFitnessTableFilter<GPXEntry> filter = new GPXTableFilter();
            //IPIFitnessTableReader<GPXEntry> fitnessReader = new GPXDbReader(gpxRepository, filter);

            //AFDatabase database = new PISystems()["BSHANGE6430s"].Databases["PIFitness"];
            //PIFitnessLog.Write(TraceEventType.Information, 0, "Connected to AF database");

            //AFElementLookup elementLookup = new AFElementLookup(database);
            //IPIFitnessRowProcessor<GPXEntry> rowProcessor = new GPXRowProcessor(elementLookup);

            //IPIFitnessValueWriter valueWriter = new PIFitnessValueWriter();
            //IPIFitnessProcessor fitnessProcessor = new GPXProcessor(fitnessReader, rowProcessor, valueWriter);

            //IList<IPIFitnessProcessor> processorList = new List<IPIFitnessProcessor>();
            //processorList.Add(fitnessProcessor);
            //_serviceWorker = new ServiceWorker(processorList);

            IKernel kernel = new StandardKernel(new GpxModule(), new DomainModule());
            
            IPIFitnessProcessor gpxProcessor = kernel.Get<GPXProcessor>();

            _serviceWorker = kernel.Get<ServiceWorker>();
            _serviceWorker.AddProcessor(gpxProcessor);
            

            BeginOperation();
        }

        private void BeginOperation()
        {
            try
            {
                if (Environment.UserInteractive)
                {
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["pollOnce"]))
                    {
                        _serviceWorker.DoWork(new object());
                    }
                    else
                    {
                        SetUpTimer();           
                    }
                    Console.WriteLine("Press any key to stop program");
                    Console.ReadKey();
                }
                else
                {
                    SetUpTimer();
                }
            }
            catch (Exception ex)
            {
                PIFitnessLog.Write(TraceEventType.Error, 2, ex);
            }

        }

        private void SetUpTimer()
        {
            try
            {
                //Now set up timer
                _timer = new System.Timers.Timer();
                _timer.Elapsed += StartPoll;

                pollingInterval = Convert.ToDouble(ConfigurationManager.AppSettings["pollingInterval"]);
                pollingOffset = Convert.ToDouble(ConfigurationManager.AppSettings["pollingOffset"]);

                _timer.Interval = FindInterval();

                _timer.Start();
            }
            catch (Exception ex)
            {
                PIFitnessLog.Write(TraceEventType.Error, 2, ex);
            }
        }

        private double FindInterval()
        {
            double totalMilliseconds;

            DateTime currentTime = DateTime.Now;
            double seconds = currentTime.Second;
            double secondsToAdd = pollingInterval - (seconds % pollingInterval) + pollingOffset;
            double milliseconds = currentTime.Millisecond;

            projected = currentTime.AddSeconds(secondsToAdd).AddMilliseconds(-milliseconds);

            totalMilliseconds = (projected - currentTime).TotalMilliseconds;

            PIFitnessLog.Write(TraceEventType.Information, 0,
                string.Format("Next poll at {0}", projected.ToString()));

            return totalMilliseconds;
        }

        private void StartPoll(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();
            _serviceWorker.Poll(sender, e);
            _timer.Interval = FindInterval();
            _timer.Start();
        }

        protected override void OnStop()
        {
            PIFitnessLog.Close();
            if (this._timer != null)
            {
                this._timer.Stop();
                this._timer = null;
            }
        }

        public void ConsoleStart(string[] args)
        {
            OnStart(args);
        }

        public void ConsoleStop()
        {
            OnStop();
        }
    }
}
