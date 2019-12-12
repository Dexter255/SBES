using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class WCFClient : ChannelFactory<IWCFService>, IWCFService, IDisposable
    {
        IWCFService factory;

        public WCFClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {

            //uzimam windows naziv korisnika koji je pokrenuo ovo i dobijem nesto tipa xxx\userAdmin i formater mi vrati userAdmin
            String clientCertificateCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            //podesavamo u channel factory da mod validacije bude chaintrust i da se revokacija ne proverava (da li je sertifikat povucen -> NoCheck)
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            
            //postavljam i svoj sertifikat (client)
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, clientCertificateCN);
            factory = this.CreateChannel();

        }

        #region Admin's permissions
        public bool CreateDatabase(string databaseName)
        {
            //Debugger.Launch();
            try
            {
                return factory.CreateDatabase(databaseName);
            }
            // ovo baca kod CheckAccessCore u okviru AuthorizationManager-a klase ukoliko korisnik nema dozvolu
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("\nError while trying to create database: " + e.Message + Environment.NewLine);
                return false;
            }
            // ovo baca svaka metode u okviru WCFService klase 
            catch(FaultException e)
            {
                Console.WriteLine("\nError while trying to create database: " + e.Message);
                return false;
            }
        }

        public bool DeleteDatabase(string databaseName)
        {
            try
            {
                return factory.DeleteDatabase(databaseName);
            }
            catch(SecurityAccessDeniedException e)
            {
                Console.WriteLine("\nError while trying to delete database: " + e.Message + Environment.NewLine);
                return false;
            }
            catch (FaultException e)
            {
                Console.WriteLine("\nError while trying to delete database: " + e.Message);
                return false;
            }
        }
        #endregion

        #region Modifier's permissions
        public bool Edit(string databaseName, int id, string country, string city, short age, double salary, string payDay)
        {
            try
            {
                return factory.Edit(databaseName, id, country, city, age, salary, payDay);
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to edit entity in database: " + e.Message);
                return false;
            }
            catch (FaultException e)
            {
                Console.WriteLine("Error while trying to edit entity in database: " + e.Message);
                return false;
            }
        }

        public bool Insert(string databaseName, string country, string city, short age, double salary, string payDay)
        {
            try
            {
                return factory.Insert(databaseName, country, city, age, salary, payDay);
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to insert entity to database: " + e.Message);
                return false;
            }
            catch (FaultException e)
            {
                Console.WriteLine("Error while trying to insert entity to database: " + e.Message);
                return false;
            }
        }
        #endregion

        #region Viewer's permissions
        public double AverageSalaryByCityAndAge(string databaseName, string city, short fromAge, short toAge)
        {
            try
            {
                return factory.AverageSalaryByCityAndAge(databaseName, city, fromAge, toAge);
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("\nError while trying to get average salary by city and age from database: " + e.Message + Environment.NewLine);
                return -1;
            }
            catch (FaultException e)
            {
                Console.WriteLine("\nError while trying to get average salary by city and age from database: " + e.Message);
                return -1;
            }
        }

        public double AverageSalaryByCountryAndPayday(string databaseName, string country, string payDay)
        {
            try
            {
                return factory.AverageSalaryByCountryAndPayday(databaseName, country, payDay);
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("\nError while trying to get average salary by country and payday from database: " + e.Message + Environment.NewLine);
                return -1;
            }
            catch (FaultException e)
            {
                Console.WriteLine("\nError while trying to get average salary by country and payday from database: " + e.Message);
                return -1;
            }
        }

        public string ViewAll(string databaseName)
        {
            try
            {
                return factory.ViewAll(databaseName);
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("\nError while trying to get all entities from database: " + e.Message + Environment.NewLine);
                return "";
            }
            catch (FaultException e)
            {
                Console.WriteLine("\nError while trying to get all entities from database: " + e.Message);
                return "";
            }
        }

        public string ViewMaxPayed(string databaseName, bool tf)
        {
            try
            {
                return factory.ViewMaxPayed(databaseName, tf);
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("\nError while trying to get max salary from all states: " + e.Message + Environment.NewLine);
                return "";
            }
            catch (FaultException e)
            {
                Console.WriteLine("\nError while trying to get max salary from all states: " + e.Message);
                return "";
            }
        }
        #endregion
    }
}
