using System;
using System.Diagnostics;

namespace Zus
{
    /// <summary>
    /// Simple trace listener, more on it:
    /// - https://docs.microsoft.com/en-us/dotnet/framework/wcf/diagnostics/configuring-message-logging?redirectedfrom=MSDN
    /// - https://dustinmarlowe.com/supidly-verbose-logging-of-wcf/
    /// </summary>
    public class ConsoleTraceListener : TraceListener
    {
        public void LogRawXml(string xml, string message)
        {
            Console.WriteLine($"Message{message}\nXML:{xml}");
        }

        public void LogMessage(string message)
        {
            this.LogRawXml(string.Empty, message);
        }

        public override void Write(string message)
        {
            this.LogMessage(message);
        }

        public override void WriteLine(string message)
        {
            this.LogRawXml(message, "WCF Service Request/Response");
        }
    }
}
