using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using System.Timers;

using PIFitness.Log;
using PIFitness.Domain;
using PIFitness.Domain.Interfaces;

namespace PIFitness.Service
{
    public class ServiceWorker
    {
        private IList<IFitnessProcessor> _processorList;

        public ServiceWorker(IList<IFitnessProcessor> processorList)
        {
            this._processorList = processorList;
        }

        public void AddProcessor(IFitnessProcessor processor)
        {
            _processorList.Add(processor);
        }

        public void Poll(object sender, ElapsedEventArgs e)
        {
            PIFitnessLog.Write(TraceEventType.Verbose, 0, String.Format(@"Entering {0}", MethodBase.GetCurrentMethod().Name));
            DoWork(sender);
        }

        public void DoWork(object o)
        {
            foreach (var processor in _processorList)
            {
                processor.Process();
            }
        }

    }



}