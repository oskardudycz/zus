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
                    new RemoteCertificateValidationCallback((sender, certificate, chain, errors) => true);

            var client = new NawsUslugiClient();
            client.ChannelFactory.Endpoint.EndpointBehaviors.Add(new InspectorBehavior());

            var bytes = new byte[100];
            uint result = 0;

            client.CheckTransmision(bytes, 100, ref bytes, ref result);


            string skrotPrzesylki = string.Empty;
            string identyfikator = string.Empty;
            try
            {
                client.WyslijPrzesylke(bytes, 100, "producent", "oprogramowanie", "wersja", "SHA", "SDWI3.CMS.ASIC.CADESB.KEDUXML", 1, ref skrotPrzesylki, ref identyfikator);

                Console.WriteLine($"Udało się wysłać przesyłkę. Skrót: {skrotPrzesylki}. Identyfikator: {identyfikator}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR! Nie udało się wysłać przesyłki: {ex.Message}");
            }

            string strIdZadania = null;
            DateTime dataWpisu = DateTime.MinValue;
            string strTyp = null;
            uint uiWielkoscPrzesylki = 0;
            byte[] byPrzesylka = null;
            string strB64Skrot = null;

            try
            {
                client.PobierzPotwierdzenie(identyfikator, "producent", "oprogramowanie", "wersja", ref strIdZadania, ref dataWpisu, ref strTyp, ref uiWielkoscPrzesylki, ref byPrzesylka, ref strB64Skrot);

                Console.WriteLine($"Udało się pobrać potwierdzenie. IdZadania: {strIdZadania}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR! Nie udało się pobrać potwierdzenia: {ex.Message}");
            }


            try
            {
                MessageIndex msgIndex = null;
                client.PobierzIndexPrzesylek(strIdZadania, "producent", "oprogramowanie", "wersja", ref msgIndex);

                Console.WriteLine($"Udało się pobrać indeks przesylek.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR! Nie udało się pobrać indeksu przesylek: {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}
