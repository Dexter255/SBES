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
        bool CreateDatabase(String filename);
        [OperationContract]
        bool DeleteDatabase(String filename);

        [OperationContract]
        bool Insert(String country, String city, short age, double salary, String payDay);
        [OperationContract]
        bool Edit( int id, String country, String city, short age, double salary, String payDay);


        [OperationContract]
        bool View();
        [OperationContract]
        bool View(bool tf);
        [OperationContract]
        bool View(String city, short fromAge, short toAge);
        [OperationContract]
        bool View(String country, String payDay);
    }
}
