using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {

            WCFDatabase db = WCFDatabase.InitializeDb();

            //uzmemo username od servera kako bismo uzeli certificate uz pomoc toga
            String serviceCertificateCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            string address = "net.tcp://localhost:9999/WCFService";

            ServiceHost serviceHost = new ServiceHost(typeof(WCFService));
            serviceHost.AddServiceEndpoint(typeof(IWCFService), binding, address);

            //kazemo da ne gleda da li je povucen sertifikat i setujemo .cer fajl od servera
            serviceHost.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            serviceHost.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            serviceHost.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, serviceCertificateCN);

            serviceHost.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            serviceHost.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });


            serviceHost.Authorization.ServiceAuthorizationManager = new AuthorizationManager();

            //polisa sadrzi uslove koje omogucavaju evaluaciju korisnika(da li ima pravo pristupa nekoj metodi)
            //na osnovu polise radimo proveru
            serviceHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomPolicy());
            serviceHost.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            
            ///////////////////////// LOGGER /////////////////////////
            NetTcpBinding bindingLogger = new NetTcpBinding();
            bindingLogger.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            string addressLogger = "net.tcp://localhost:10000/WCFLogger";
            EndpointAddress endpointAddress = new EndpointAddress(new Uri(addressLogger));


            //WCFService proxy = new WCFService(binding, endpointAddress);
            //////////////////////////////////////////////////////////

            WCFService.InitializeService(bindingLogger, endpointAddress);
            serviceHost.Open();
            Console.WriteLine("WCFService is opened. Press <enter> to finish and save databases...");
            Console.ReadLine();

            serviceHost.Close();

            db.SerializeData();
        }
    }
}
