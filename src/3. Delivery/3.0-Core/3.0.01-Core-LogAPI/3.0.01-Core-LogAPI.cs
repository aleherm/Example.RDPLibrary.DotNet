using Refinitiv.DataPlatform;
using Refinitiv.DataPlatform.Core;
using System;

namespace _3._0._01_LogAPI
{
    // *******************************************************************************************************************************************
    // Log API
    // The Refinitiv Data Platform Library for .NET, by default, will send general information log messages to a unique log file.  For
    // applications that utilize their own logging services, the RDP for .NET log messages can be captured and rerouted for application use.
    // 
    // The following tutorial demonstrates the ability for an application to programmatically manage and control the RDP for .NET library logs.
    // *******************************************************************************************************************************************
    class Program
    {
        static void Main(string[] args)
        {
            // Programmatically override the default log level defined for the Refinitiv Library.
            Log.Level = NLog.LogLevel.Debug;

            // Intercept all Refinitiv Data Platform Library log messages within a lambda expression. In our case, the lambda expression 
            // simply echos all log messages generated within the library to the console.
            Log.Output = (loginfo, parms) => Console.WriteLine($"Application: {loginfo.Level} - {loginfo.FormattedMessage}");

            // Create the platform session.
            using (ISession session = Configuration.Sessions.GetSession())
            {
                // Open the session - a number of log messages will be generated.
                session.Open();
            }
        }
    }
}
