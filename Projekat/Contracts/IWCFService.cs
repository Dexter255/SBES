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
        #region Admin's permissions
        [OperationContract]
        bool CreateDatabase(string databaseName);
        [OperationContract]
        bool DeleteDatabase(string databaseName);
        #endregion

        #region Modifier's permissions
        [OperationContract]
        bool Edit(string databaseName, int id, string country, string city, short age, double salary, string payDay);
        [OperationContract]
        bool Insert(string databaseName, string country, string city, short age, double salary, string payDay);
        #endregion

        #region Viewer's permission
        [OperationContract]
        string ViewAll(string databaseName);
        [OperationContract]
        string ViewMaxPayed(string databaseName);
        [OperationContract]
        double AverageSalaryByCityAndAge(string databaseName, string city, short fromAge, short toAge);
        [OperationContract]
        double AverageSalaryByCountryAndPayday(string databaseName, string country, string payDay);
        #endregion
    }

}
