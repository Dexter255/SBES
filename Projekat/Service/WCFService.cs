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
    public class WCFService : IWCFService
    {
        private WCFDatabase db = WCFDatabase.InitializeDb();
        public WCFService()
        {

        }

        #region Admin's permissions
        public bool CreateDatabase(string databaseName)
        {
            //Debugger.Launch();
            if (Thread.CurrentPrincipal.IsInRole("CreateDB"))
            {

                return db.CreateDatabase(databaseName);

            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized.\n"));
            }
        }

        public bool DeleteDatabase(string databaseName)
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

        #region Modifier's permissions
        public bool Edit(string databaseName, int id, string country, string city, short age, double salary, string payDay)
        {
            if (Thread.CurrentPrincipal.IsInRole("Edit"))
            {

                return db.Edit(databaseName, id, country, city, age, salary, payDay);

            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized.\n"));
            }
        }

        public bool Insert(string databaseName, string country, string city, short age, double salary, string payDay)
        {
            if (Thread.CurrentPrincipal.IsInRole("Insert"))
            {

                return db.Insert(databaseName, country, city, age, salary, payDay);
               
            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized.\n"));
            }
        }
        #endregion

        #region Viewer's permissions
        public String ViewAll(string databaseName)
        {
            if (Thread.CurrentPrincipal.IsInRole("View"))
            {
                
                return db.ViewAll(databaseName);

            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized.\n"));
            }
        }

        public String ViewMaxPayed(string databaseName)
        {
            if (Thread.CurrentPrincipal.IsInRole("View"))
            {

                return db.ViewMaxPayed(databaseName);

            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized.\n"));
            }
        }

        public double AverageSalaryByCityAndAge(string databaseName, String city, short fromAge, short toAge)
        {
            if (Thread.CurrentPrincipal.IsInRole("View"))
            {

                return db.AverageSalaryByCityAndAge(databaseName, city, fromAge, toAge);

            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized.\n"));
            }
        }

        public double AverageSalaryByCountryAndPayday(string databaseName, String country, String payDay)
        {
            if (Thread.CurrentPrincipal.IsInRole("View"))
            {
                return db.AverageSalaryByCountryAndPayday(databaseName, country, payDay);
            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized.\n"));
            }
        }
        #endregion
    }
}
