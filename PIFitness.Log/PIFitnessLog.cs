using System;
using System.Diagnostics;

namespace PIFitness.Log
{
    public static class PIFitnessLog
    {
        public static TraceSource TraceSrc;
        public static EventLogTraceListener EventLogListener;

        public static void ConfigureLogging()
        {
            if (!EventLog.SourceExists("PI Fitness"))
            {
                EventLog.CreateEventSource("PI Fitness", "PI Fitness Service");
            }

            TraceSrc = new TraceSource("fitnessServiceSource");

            EventLogListener = (EventLogTraceListener)TraceSrc.Listeners["eventLogListener"];
            EventLogListener.EventLog.Log = "PI Fitness Service";
        }

        public static void Write(TraceEventType eventType, int id, Exception ex)
        {
            string message = String.Format("{0} {1}", ex.Message, ex.StackTrace);
            TraceSrc.TraceEvent(eventType, id, message);
        }

        public static void Write(TraceEventType eventType, int id, string message)
        {
            TraceSrc.TraceEvent(eventType, id, message);
        }

        public static void Close()
        {
            TraceSrc.Close();
        }

    }
}
