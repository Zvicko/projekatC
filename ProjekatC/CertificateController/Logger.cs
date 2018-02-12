using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateController
{
    public class Logger : IDisposable
    {
        private static EventLog customLog = null;
        const string sourceName = "CertificateController";
        const string logName = "Logger";

        /// <summary>
        /// Pravljenje Windows Event Log datoteke.
        /// </summary>
        static Logger()
        {
            try
            {
                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, logName);
                }

                customLog = new EventLog(logName, Environment.MachineName, sourceName);
            }
            catch (Exception e)
            {
                customLog = null;

                Console.WriteLine("Error while trying to create log handle. Error: {0}.", e.Message);
            }
        }

        /// <summary>
        /// Belezenje dogadjaja u logger.
        /// </summary>
        /// <param name="message"> Poruka o dogadjaju koju je potrebno zapisati u log datoteku. </param>
        public static void AnnotateEvent(string message)
        {
            customLog.WriteEntry(message);
        }

        /// <summary>
        /// Funkcija koja oslobadja resurse zauzete od strane logger-a.
        /// </summary>
        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}
