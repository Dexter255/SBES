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
        string CreateDatabase(string databaseName);
        [OperationContract]
        string DeleteDatabase(string databaseName);
        #endregion

        #region Modifier's permissions
        [OperationContract]
        string Edit(string message, byte[] signature);
        [OperationContract]
        string Insert(string message, byte[] signature);
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
