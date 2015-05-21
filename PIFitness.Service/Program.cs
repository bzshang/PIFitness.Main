using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PIFitness.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            PIFitnessService service = new PIFitnessService();
            if (Environment.UserInteractive)
            {
                string[] args = null;
                service.ConsoleStart(args);
                service.ConsoleStop();
            }
            else
            {
                ServiceBase.Run(service);
            }
        }
    }
}
