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
        bool CreateDatabase(string filename);
        [OperationContract]
        bool DeleteDatabase(string filename);
        #endregion

        #region Modifier's permissions
        [OperationContract]
        bool Edit(int id, string country, string city, short age, double salary, string payDay);
        [OperationContract]
        bool Insert(string country, string city, short age, double salary, string payDay);
        #endregion

        #region Viewer's permission
        [OperationContract]
        string ViewAll();
        [OperationContract]
        string ViewMaxPayed(bool tf);
        [OperationContract]
        double AverageSalaryByCityAndAge(string city, short fromAge, short toAge);
        [OperationContract]
        double AverageSalaryByCountryAndPayday(string country, string payDay);
        #endregion
    }

}
