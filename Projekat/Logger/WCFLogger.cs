using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logger
{
    public class WCFLogger : IWCFLogger, IDisposable
    {

        private static EventLog customLog = null;
        const string SourceName = "LoggedData";
        const string LogName = "Application";

        static WCFLogger()
        {
            //Debugger.Launch();
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }

                customLog = new EventLog(LogName, Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }

        public void AutentificationFailed(string userName)
        {
            if (customLog != null)
            {
                string message = $"User '{userName}': authentification failed!" + Environment.NewLine;
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException("Error while trying to write event (authentification failed) to event log.");
            }
        }

        public void AutentificationSuccess(string userName)
        {

            if (customLog != null)
            {
                string message = $"User '{userName}': authentification success!" + Environment.NewLine;
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException("Error while trying to write event (authentification success) to event log.");
            }
        }

        public void AuthorizationFailed(string userName, string reason)
        {
            if (customLog != null)
            {
                string message = $"User '{userName}': authentification failed! Tride to access service 'wcfService'" + Environment.NewLine + $"Reason: {reason}";
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException("Error while trying to write event (authorization failed) to event log.");
            }
        }

        public void AuthorizationSuccess(string userName)
        {
            //Debugger.Launch();
            if (customLog != null)
            {
                string message = $"User '{userName}': authorization success!" + Environment.NewLine;
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException("Error while trying to write event (authorization success) to event log.");
            }
        }

        public void UserOperation(string userName, string method, string information)
        {
            //Debugger.Launch();
            if (customLog != null)
            {
                string message = $"User '{userName}' called:\nMethod: {method}\nInformation: {information}";
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException($"Error while trying to write event {method} to event log.");
            }
        }

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
