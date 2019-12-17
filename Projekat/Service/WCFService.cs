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
using WCFService_Client;

namespace Service
{
    public class WCFService : ChannelFactory<IWCFLogger>, IWCFService
    {
        private WCFDatabase db = WCFDatabase.InitializeDb();
        
        public WCFService()
        {

        }

        #region Admin's operations
        public string CreateDatabase(string databaseName)
        {
            //Debugger.Launch();
            string clientName = ((Thread.CurrentPrincipal as CustomPrincipal).Identity).Name.Split(',', ';')[0].Split('=')[1];

            if (Thread.CurrentPrincipal.IsInRole("CreateDB"))
            {
                WCFServiceLoggerConnection.Factory.AuthorizationSuccess(clientName);

                string information = db.CreateDatabase(databaseName);
                WCFServiceLoggerConnection.Factory.UserOperation(clientName, "Create database", information);

                return information;
            }
            else
            {
                WCFServiceLoggerConnection.Factory.AuthorizationFailed(clientName, "Not authorized to create database.");

                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized.\n"));
            }
        }

        public string DeleteDatabase(string databaseName)
        {
            string clientName = ((Thread.CurrentPrincipal as CustomPrincipal).Identity).Name.Split(',', ';')[0].Split('=')[1];

            if (Thread.CurrentPrincipal.IsInRole("DeleteDB"))
            {
                WCFServiceLoggerConnection.Factory.AuthorizationSuccess(clientName);

                string information = db.DeleteDatabase(databaseName);
                WCFServiceLoggerConnection.Factory.UserOperation(clientName, "Delete database", information);

                return information;
            }
            else
            {
                WCFServiceLoggerConnection.Factory.AuthorizationFailed(clientName, "Not authorized to delete database.");

                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized.\n"));
            }
        }
        #endregion

        #region Modifier's operations
        public string Edit(string message, byte[] signature)
        {
            string clientName = ((Thread.CurrentPrincipal as CustomPrincipal).Identity).Name.Split(',', ';')[0].Split('=')[1];

            if (Thread.CurrentPrincipal.IsInRole("Edit"))
            {
                WCFServiceLoggerConnection.Factory.AuthorizationSuccess(clientName);

                string information = db.Edit(message, signature);
                WCFServiceLoggerConnection.Factory.UserOperation(clientName, "Edit entity", information);

                return information;
            }
            else
            {
                WCFServiceLoggerConnection.Factory.AuthorizationFailed(clientName, "Not authorized to edit entity in database.");

                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized.\n"));
            }
        }

        public string Insert(string message, byte[] signature)
        {
            string clientName = ((Thread.CurrentPrincipal as CustomPrincipal).Identity).Name.Split(',', ';')[0].Split('=')[1];

            if (Thread.CurrentPrincipal.IsInRole("Insert"))
            {
                WCFServiceLoggerConnection.Factory.AuthorizationSuccess(clientName);

                string information = db.Insert(message, signature);
                WCFServiceLoggerConnection.Factory.UserOperation(clientName, "Insert new entity", information);

                return information;
            }
            else
            {
                WCFServiceLoggerConnection.Factory.AuthorizationFailed(clientName, "Not authorized to insert new entity in database.");

                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized.\n"));
            }
        }
        #endregion

        #region Viewer's operations
        public byte[] ViewAll(string databaseName)
        {
            string clientName = ((Thread.CurrentPrincipal as CustomPrincipal).Identity).Name.Split(',', ';')[0].Split('=')[1];
            WCFServiceLoggerConnection.Factory.AuthorizationSuccess(clientName);

            string information = $"Got 'em all";
            WCFServiceLoggerConnection.Factory.UserOperation(clientName, $"Get all entities from database '{databaseName}'", information);

            return db.ViewAll(databaseName);

        }

        public byte[] ViewMaxPayed(string databaseName)
        {
            string clientName = ((Thread.CurrentPrincipal as CustomPrincipal).Identity).Name.Split(',', ';')[0].Split('=')[1];
            WCFServiceLoggerConnection.Factory.AuthorizationSuccess(clientName);

            string information = $"Got 'em all";
            WCFServiceLoggerConnection.Factory.UserOperation(clientName, $"Get max salary from all states from database '{databaseName}'", information);

            return db.ViewMaxPayed(databaseName);
        }

        public byte[] AverageSalaryByCityAndAge(string databaseName, String city, short fromAge, short toAge)
        {
            string clientName = ((Thread.CurrentPrincipal as CustomPrincipal).Identity).Name.Split(',', ';')[0].Split('=')[1];
            WCFServiceLoggerConnection.Factory.AuthorizationSuccess(clientName);

            string information = $"Got 'em all";
            WCFServiceLoggerConnection.Factory.UserOperation(clientName, $"Get average salary by city and age from database '{databaseName}'", information);

            return db.AverageSalaryByCityAndAge(databaseName, city, fromAge, toAge);
        }

        public byte[] AverageSalaryByCountryAndPayday(string databaseName, String country, String payDay)
        {
            string clientName = ((Thread.CurrentPrincipal as CustomPrincipal).Identity).Name.Split(',', ';')[0].Split('=')[1];
            WCFServiceLoggerConnection.Factory.AuthorizationSuccess(clientName);

            string information = $"Got 'em all";
            WCFServiceLoggerConnection.Factory.UserOperation(clientName, $"Get average salary by country and payday from database '{databaseName}'", information);

            return db.AverageSalaryByCountryAndPayday(databaseName, country, payDay);
        }

        public byte[] ViewDatabasesNames()
        {
            string clientName = ((Thread.CurrentPrincipal as CustomPrincipal).Identity).Name.Split(',', ';')[0].Split('=')[1];
            WCFServiceLoggerConnection.Factory.AuthorizationSuccess(clientName);

            string information = $"Got 'em all";
            WCFServiceLoggerConnection.Factory.UserOperation(clientName, $"Get databases names", information);

            return db.ViewDatabasesNames();
        }

        #endregion
    }
}
