using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

            serviceHost.Open();
            Console.WriteLine("WCFService is opened. Press <enter> to finish...");
            Console.ReadLine();

            serviceHost.Close();
        }
    }
}
