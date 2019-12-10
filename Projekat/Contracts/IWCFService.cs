using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface IWCFService
    {

        [OperationContract]
        double AverageSalaryByCityAndAge(string city, short fromAge, short toAge);
        [OperationContract]
        double AverageSalaryByCountryAndPayday(string country, string payDay);
        [OperationContract]
        bool CreateDatabase(string filename);
        [OperationContract]
        bool DeleteDatabase(string filename);
        [OperationContract]
        bool Edit(int id, string country, string city, short age, double salary, string payDay);
        [OperationContract]
        bool Insert(string country, string city, short age, double salary, string payDay);
        [OperationContract]
        string ViewAll();
        [OperationContract]
        string ViewMaxPayed(bool tf);
    }
}
