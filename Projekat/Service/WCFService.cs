using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public class WCFService : ChannelFactory<IWCFLogger>, IWCFService
    {
        static private IWCFLogger factory;

        private WCFDatabase db = WCFDatabase.InitializeDb();
        private WCFService(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public static IWCFLogger InitializeService(NetTcpBinding binding, EndpointAddress address)
        {
            if (factory == null)
            {
                var service = new WCFService(binding, address);
                //factory = service.factory;
            }

            return factory;
        }

        public WCFService()
        {

        }

        #region Admin's operations
        public string CreateDatabase(string databaseName)
        {
            Debugger.Launch();
            string clientName = ((Thread.CurrentPrincipal as CustomPrincipal).Identity).Name.Split(',', ';')[0].Split('=')[1];

            if (Thread.CurrentPrincipal.IsInRole("CreateDB"))
            {
                factory.AuthorizationSuccess(clientName);
                
                return db.CreateDatabase(databaseName);
            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized.\n"));
            }
        }

        public string DeleteDatabase(string databaseName)
        {
            if (Thread.CurrentPrincipal.IsInRole("DeleteDB"))
            {
                return db.DeleteDatabase(databaseName);
            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized.\n"));
            }
        }
        #endregion

        #region Modifier's operations
        public string Edit(string message, byte[] signature)
        {
            if (Thread.CurrentPrincipal.IsInRole("Edit"))
            {
                return db.Edit(message, signature);
            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized.\n"));
            }
        }

        public string Insert(string message, byte[] signature)
        {
            if (Thread.CurrentPrincipal.IsInRole("Insert"))
            {
                return db.Insert(message, signature);
            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized.\n"));
            }
        }
        #endregion

        #region Viewer's operations
        public byte[] ViewAll(string databaseName)
        {
            return db.ViewAll(databaseName);
        }

        public byte[] ViewMaxPayed(string databaseName)
        {
            return db.ViewMaxPayed(databaseName);
        }

        public byte[] AverageSalaryByCityAndAge(string databaseName, String city, short fromAge, short toAge)
        {
            return db.AverageSalaryByCityAndAge(databaseName, city, fromAge, toAge);
        }

        public byte[] AverageSalaryByCountryAndPayday(string databaseName, String country, String payDay)
        {
            return db.AverageSalaryByCountryAndPayday(databaseName, country, payDay);
        }

        public byte[] ViewDatabasesNames()
        {
            return db.ViewDatabasesNames();
        }

        #endregion
    }
}
