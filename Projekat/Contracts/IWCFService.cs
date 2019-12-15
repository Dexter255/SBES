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
        byte[] ViewAll(string databaseName);
        [OperationContract]
        byte[] ViewMaxPayed(string databaseName);
        [OperationContract]
        byte[] AverageSalaryByCityAndAge(string databaseName, string city, short fromAge, short toAge);
        [OperationContract]
        byte[] AverageSalaryByCountryAndPayday(string databaseName, string country, string payDay);
        #endregion
    }

}
