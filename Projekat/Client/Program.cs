using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/WCFService";

            using (WCFClient proxy = new WCFClient(binding, new EndpointAddress(new Uri(address))))
            {
                proxy.CreateDatabase("baza1");
                proxy.DeleteDatabase("baza1");
                proxy.Insert("SRB", "Novi Sad", 18, 256.24, "2019");
                proxy.Edit(1, "SRB", "Novi Sad", 18, 256.24, "2019");
                proxy.View();
                proxy.View(true);
                proxy.View("Novi Sad", 18, 24);
                proxy.View("SRB", "2019");
            }

            Console.ReadLine();
        }
    }
}
