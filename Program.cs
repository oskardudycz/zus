using System;
using System.Net;
using System.Net.Security;
using Zus.ServiceReference1;

namespace Zus
{
    internal class Program
    {
        const bool disableCertificateValidation = true;

        static void Main(string[] args)
        {
            if (disableCertificateValidation)
                ServicePointManager.ServerCertificateValidationCallback +=
                    new RemoteCertificateValidationCallback((sender, certificate, chain, errors) =>true);

            var client = new NawsUslugiClient();

            var bytes = new byte[100];
            uint result = 0;

            client.CheckTransmision(bytes, 100, ref bytes, ref result);
            Console.ReadLine();
        }
    }
}
