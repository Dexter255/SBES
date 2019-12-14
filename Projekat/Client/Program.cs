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
                    SelectOption(proxy, option);
                } while (option != "9");
            }

            Console.WriteLine("\nPress any key to exit.");
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
            bool successfull = false;
            double returnedValueDouble;
            double salary;
            short fromAge;
            short toAge;

            if(option != "9")
            {
                Console.Write("\nEnter database name: ");
                databaseName = Console.ReadLine();
            }

            // povratna vrednost f-ja bi trebala biti int (enum)
            // -1 za exception
            // 0 za neuspesno - kada vec postoji/nepostoji baza podataka sa prosledjenim imenom 
            // 1 za uspesno
            switch (option)
            {
                case "1":
                    // -2 = za exception
                    // -1 (neuspesno) = baza podataka sa nazivom ne postoji 
                    // 1 (uspesno)
                    successfull = proxy.CreateDatabase(databaseName);
                    if (successfull)
                        Console.WriteLine("Database successfully created.\n");
                    else
                        Console.WriteLine($"Database with name '{databaseName}' already exists.\n");

                    break;

                case "2":
                    // -2 = za exception
                    // -1 (neuspesno) = baza podataka sa nazivom ne postoji 
                    // 1 (uspesno)
                    successfull = proxy.DeleteDatabase(databaseName);
                    if (successfull)
                        Console.WriteLine("Database successfully deleted.\n");
                    else
                        Console.WriteLine($"Database with name '{databaseName}' doesn't exists.\n");

                    break;

                case "3":
                    Console.Write("Country: ");
                    country = Console.ReadLine();

                    Console.Write("City: ");
                    city = Console.ReadLine();

                    do
                    {
                        Console.Write("Age: ");
                        temp = Console.ReadLine();
                    } while (!short.TryParse(temp, out fromAge));

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

                    // -2 = za exception
                    // -1 (neuspesno) = baza podataka sa nazivom ne postoji 
                    // 1 (uspesno)
                    successfull = proxy.Insert(databaseName, country, city, fromAge, salary, payday);
                    if (successfull)
                        Console.WriteLine("New entity successfully inserted.\n");
                    else
                        Console.WriteLine($"Database with name '{databaseName}' doesn't exists.\n");

                    break;

                case "4":
                    int id;
                    do
                    {
                        Console.Write("ID: ");
                    } while (!Int32.TryParse(Console.ReadLine(), out id));

                    Console.Write("Country: ");
                    country = Console.ReadLine();

                    Console.Write("City: ");
                    city = Console.ReadLine();

                    do
                    {
                        Console.Write("Age: ");
                        temp = Console.ReadLine();
                    } while (!short.TryParse(temp, out fromAge));

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

                    // -2 = za exception
                    // -1 (neuspesno) = baza podataka sa nazivom ne postoji 
                    // 0 (neuspesno) = id ne postoji
                    // 1 (uspesno)
                    successfull = proxy.Edit(databaseName, id, country, city, fromAge, salary, payday);
                    if (successfull)
                        Console.WriteLine("Existing entity successfully edited.\n");
                    else /*if*/
                        Console.WriteLine($"Database with name '{databaseName}' doesn't exists.\n");
                    /*else if
                        Console.WriteLine($"Entity with id '{id}' doesn't exists.\n");*/

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
                    } while (!Int32.TryParse(payday, out id));

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
    }
}
