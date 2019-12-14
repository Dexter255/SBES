using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

                    if (Int32.TryParse(option, out int opt) && option != "9")
                        SelectOption(proxy, option);

                } while (option != "9");
            }

            Console.Write("\nPress any key to exit.");
            Console.ReadLine();
        }

        private static void SelectOption(WCFClient proxy, string option)
        {
            Debugger.Launch();
            string databaseName = String.Empty;
            string returnedValueString = String.Empty;
            string city = String.Empty;
            string country = String.Empty;
            string payday = String.Empty;
            string temp = String.Empty;
            string message = String.Empty;
            byte[] signature;
            double returnedValueDouble;
            short fromAge;
            short toAge;

            Console.Write("\nEnter database name: ");
            databaseName = Console.ReadLine();

            switch (option)
            {
                case "1":
                    returnedValueString = proxy.CreateDatabase(databaseName);
                    Console.WriteLine(Environment.NewLine + returnedValueString);

                    break;

                case "2":
                    returnedValueString = proxy.DeleteDatabase(databaseName);
                    Console.WriteLine(Environment.NewLine + returnedValueString);

                    break;

                case "3":
                    message = CreateMessage(databaseName, "Insert");

                    signature = DigitalSignature.Create(message, proxy.Credentials.ClientCertificate.Certificate);

                    returnedValueString = proxy.Insert(message, signature);
                    Console.WriteLine(Environment.NewLine + returnedValueString);

                    break;

                case "4":
                    message = CreateMessage(databaseName, "Edit");

                    signature = DigitalSignature.Create(message, proxy.Credentials.ClientCertificate.Certificate);

                    returnedValueString = proxy.Edit(message, signature);
                    Console.WriteLine(Environment.NewLine + returnedValueString);

                    break;

                case "5":
                    returnedValueString = proxy.ViewAll(databaseName);
                    if (returnedValueString != "-1")
                        Console.WriteLine($"Entities: \n{returnedValueString}\n");

                    break;

                case "6":
                    returnedValueString = proxy.ViewMaxPayed(databaseName);
                    if (returnedValueString != "-1")
                        Console.WriteLine($"Max salary from all states: \n{returnedValueString}\n");

                    break;

                case "7":
                    Console.Write("Country: ");
                    country = Console.ReadLine();

                    do
                    {
                        Console.Write("Payday: ");
                        payday = Console.ReadLine();
                    } while (!Int32.TryParse(payday, out int id));

                    returnedValueDouble = proxy.AverageSalaryByCountryAndPayday(databaseName, country, payday);
                    if (returnedValueDouble != -1)
                        Console.WriteLine($"Avarage salary by country and payday: {returnedValueDouble}\n");

                    break;

                case "8":
                    Console.Write("City: ");
                    city = Console.ReadLine();

                    do
                    {
                        do
                        {
                            Console.Write("From age: ");
                            temp = Console.ReadLine();
                        } while (!short.TryParse(temp, out fromAge));

                        do
                        {
                            Console.Write("To age: ");
                            temp = Console.ReadLine();
                        } while (!short.TryParse(temp, out toAge));
                    } while (fromAge > toAge);

                    returnedValueDouble = proxy.AverageSalaryByCityAndAge(databaseName, city, fromAge, toAge);
                    if (returnedValueDouble != -1)
                        Console.WriteLine($"Avarage salary by city and age: {returnedValueDouble}\n");

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

        private static string CreateMessage(string databaseName, string operation)
        {
            List<object> messageAndSignature = new List<object>();
            string temp;
            string payday;
            double salary;
            int id = -1;

            short age;

            if (operation == "Edit")
            {
                do
                {
                    Console.Write("ID: ");
                } while (!Int32.TryParse(Console.ReadLine(), out id));
            }

            Console.Write("Country: ");
            string country = Console.ReadLine();

            Console.Write("City: ");
            string city = Console.ReadLine();

            do
            {
                Console.Write("Age: ");
                temp = Console.ReadLine();
            } while (!short.TryParse(temp, out age));

            do
            {
                Console.Write("Salary: ");
                temp = Console.ReadLine();
            } while (!Double.TryParse(temp, out salary));

            do
            {
                Console.Write("Payday: ");
                payday = Console.ReadLine();
            } while (!Int32.TryParse(payday, out int pay));

            string message = $"{databaseName}:{country}:{city}:{age}:{salary}:{payday}:{id}";

            return message;
        }
    }
}
