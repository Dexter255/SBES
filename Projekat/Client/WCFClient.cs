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

        public double AverageSalaryByCityAndAge(string city, short fromAge, short toAge)
        {
            return factory.AverageSalaryByCityAndAge(city, fromAge, toAge);
        }

        public double AverageSalaryByCountryAndPayday(string country, string payDay)
        {
            return factory.AverageSalaryByCountryAndPayday(country, payDay);
        }

        public bool CreateDatabase(string filename)
        {
            return factory.CreateDatabase(filename);
        }

        public bool DeleteDatabase(string filename)
        {
            return factory.DeleteDatabase(filename);
        }

        public bool Edit(int id, string country, string city, short age, double salary, string payDay)
        {
            return factory.Edit(id, country, city, age, salary, payDay);
        }

        public bool Insert(string country, string city, short age, double salary, string payDay)
        {
            return factory.Insert(country, city, age, salary, payDay);
        }

        public string ViewAll()
        {
            return factory.ViewAll();
        }

        public string ViewMaxPayed(bool tf)
        {
            return factory.ViewMaxPayed(tf);
        }
    }
}
