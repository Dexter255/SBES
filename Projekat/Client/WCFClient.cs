using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
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
            factory = this.CreateChannel();
        }

        public bool CreateDatabase(string filename)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDatabase(string filename)
        {
            throw new NotImplementedException();
        }

        public bool Edit(int id, string country, string city, short age, double salary, string payDay)
        {
            throw new NotImplementedException();
        }

        public bool Insert(String country, String city, short age, double salary, String payDay)
        {
            throw new NotImplementedException();
        }

        public bool View()
        {
            throw new NotImplementedException();
        }

        public bool View(bool tf)
        {
            throw new NotImplementedException();
        }

        public bool View(string city, short fromAge, short toAge)
        {
            throw new NotImplementedException();
        }

        public bool View(string country, string payDay)
        {
            throw new NotImplementedException();
        }
    }
}
