using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/WCFService";

            ServiceHost serviceHost = new ServiceHost(typeof(WCFService));
            serviceHost.AddServiceEndpoint(typeof(IWCFService), binding, address);

            serviceHost.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            serviceHost.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            //serviceHost.Authorization.ServiceAuthorizationManager = new AuthorizationManager();
            serviceHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomPolicy());
            serviceHost.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            serviceHost.Open();
            Console.WriteLine("WCFService is opened. Press <enter> to finish...");
            Console.ReadLine();

            serviceHost.Close();
        }
    }
}
