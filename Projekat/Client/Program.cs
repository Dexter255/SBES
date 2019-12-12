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
                string option;

                do
                {
                    PrintMenu();
                    option = Console.ReadLine();
                    SelectOption(proxy, option);
                } while (option != "9");
            }

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadLine();
        }

        private static void SelectOption(WCFClient proxy, string option)
        {
            string databaseName = String.Empty;

            switch (option)
            {
                case "1":
                    Console.Write("\nEnter database name: ");
                    databaseName = Console.ReadLine();

                    bool successfull = proxy.CreateDatabase(databaseName);
                    if (successfull)
                        Console.WriteLine("Database successfully created.\n");
                    else if(!successfull)
                        Console.WriteLine($"Database with name {databaseName} already exists.\n");

                    break;

                case "2":
                    Console.Write("\nEnter database name: ");
                    databaseName = Console.ReadLine();

                    if (proxy.DeleteDatabase(databaseName))
                        Console.WriteLine("Database successfully deleted.\n");
                    else
                        Console.WriteLine($"Database with name {databaseName} doesn't exists.\n");

                    break;

                case "3":
                    proxy.Insert(databaseName, "SRB", "Novi Sad", 18, 256.24, "2019");
                    break;

                case "4":
                    proxy.Edit(databaseName, 1, "SRB", "Novi Sad", 18, 256.24, "2019");
                    break;

                case "5":
                    proxy.ViewAll(databaseName);
                    break;

                case "6":
                    proxy.ViewMaxPayed(databaseName, true);
                    break;

                case "7":
                    proxy.AverageSalaryByCountryAndPayday(databaseName, "SRB", "2019");
                    break;

                case "8":
                    proxy.AverageSalaryByCityAndAge(databaseName, "Novi Sad", 18, 24);
                    break;

                case "9":
                    Console.WriteLine("Exit");
                    break;

                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("\t1. Create database");
            Console.WriteLine("\t2. Delete database");
            Console.WriteLine("\t3. Insert new entity");
            Console.WriteLine("\t4. Edit existing entity");
            Console.WriteLine("\t5. Get all entities");
            Console.WriteLine("\t6. Get max salary from all states");
            Console.WriteLine("\t7. Get average salary by country and payday");
            Console.WriteLine("\t8. Get average salary by city and age");
            Console.WriteLine("\t9. Exit");
            Console.Write("\t>> ");
        }
    }
}
