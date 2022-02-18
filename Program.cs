using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Zus3.ServiceReference1;

namespace Zus3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new NawsUslugiClient();

            var bytes = new byte[100];
            uint result = 0;

            client.CheckTransmision(bytes, 100, ref bytes, ref result);
        }
    }
}
