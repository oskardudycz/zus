using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
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
                    (sender, certificate, chain, errors) => true;

            var client = new NawsUslugiClient();
            client.ChannelFactory.Endpoint.EndpointBehaviors.Add(new InspectorBehavior());

            var bytes = File.ReadAllBytes("XMLEnc ASiC--XAdES-B.xml");
            uint result = 0;

            client.CheckTransmision(bytes, (uint)bytes.Length, ref bytes, ref result);


            var strNazwaProducenta = "My company";
            var strNazwaOprogramowania = "Supersoft";
            var strWersjaOprogramowania = "1.1.2";

            var strB64SkrotPrzesylkiIn = GetSha256(bytes);
            var strTypPrzesylki = "SDWI3.XMLENC.ASIC.XADESB.KEDUXML";
            int? metodaPodpisu = null;
            var strB64SkrotPrzesylkiOut = string.Empty;
            var strIdentyfikator = string.Empty;
            try
            {
                client.WyslijPrzesylke(bytes, (uint)bytes.Length, strNazwaProducenta, strNazwaOprogramowania,
                    strWersjaOprogramowania, strB64SkrotPrzesylkiIn, strTypPrzesylki, metodaPodpisu,
                    ref strB64SkrotPrzesylkiOut,
                    ref strIdentyfikator);

                Console.WriteLine(
                    $"Udało się wysłać przesyłkę. Skrót: {strB64SkrotPrzesylkiOut}. Identyfikator: {strIdentyfikator}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR! Nie udało się wysłać przesyłki: {ex.Message}");
            }
            MessageIndex msgIndex = null;
            
            try
            {
                client.PobierzIndexPrzesylek(strIdentyfikator, strNazwaProducenta, strNazwaOprogramowania, strWersjaOprogramowania, ref msgIndex);

                Console.WriteLine($"Udało się pobrać indeks przesylek.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR! Nie udało się pobrać indeksu przesylek: {ex.Message}");
            }
            
            string strIdZadania = null;
            DateTime dataWpisu = default;
            string strTyp = null;
            uint uiWielkoscPrzesylki = 0;
            byte[] byPrzesylka = null;
            string strB64Skrot = null;
            
            
            try
            {
                client.PobierzPotwierdzenie(strIdentyfikator, strNazwaProducenta, strNazwaOprogramowania, strWersjaOprogramowania, ref strIdZadania,
                    ref dataWpisu, ref strTyp, ref uiWielkoscPrzesylki, ref byPrzesylka, ref strB64Skrot);

                Console.WriteLine($"Udało się pobrać potwierdzenie. IdZadania: {strIdZadania}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR! Nie udało się pobrać potwierdzenia: {ex.Message}");
            }

            Console.ReadLine();
        }

        private static string GetSha256(byte[] bytes)
        {
            using (var mySha256 = SHA256.Create())
            {
                var hashValue = mySha256.ComputeHash(bytes);

                return Convert.ToBase64String(hashValue);
            }
        }
    }
}