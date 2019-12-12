using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //naziv serverovog .cer sertifikata
            String serverCertificateCN = "wcfService";
            NetTcpBinding binding = new NetTcpBinding();
            //setujem da se autentifikacija radi uz pomoc sertifikata
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            //kupim sertifikat iz storage-a sa tim serverskim nazivom
            X509Certificate2 serverCertificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, serverCertificateCN);
            //adresa za konekciju sa serverom
            string address = "net.tcp://localhost:9999/WCFService";
            //kreiramo endpoint i setujemo mu sertifikat
            EndpointAddress endpointAddress = new EndpointAddress(new Uri(address), new X509CertificateEndpointIdentity(serverCertificate));

            using (WCFClient proxy = new WCFClient(binding, endpointAddress))
            {
                proxy.CreateDatabase("baza1");
                proxy.DeleteDatabase("baza1");
                proxy.Insert("SRB", "Novi Sad", 18, 256.24, "2019");
                proxy.Edit(1, "SRB", "Novi Sad", 18, 256.24, "2019");
                proxy.ViewAll();
                proxy.ViewMaxPayed(true);
                proxy.AverageSalaryByCityAndAge("Novi Sad", 18, 24);
                proxy.AverageSalaryByCountryAndPayday("SRB", "2019");
            }

            Console.ReadLine();
        }
    }
}
