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
        #region Admin's permissions
        public bool CreateDatabase(string filename)
        {
            //Debugger.Launch();
            if (Thread.CurrentPrincipal.IsInRole("CreateDB"))
            {
                Console.WriteLine("Create database succeeded");
                return true;
            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized."));
            }
        }

        public bool DeleteDatabase(string filename)
        {
            if (Thread.CurrentPrincipal.IsInRole("DeleteDB"))
            {
                Console.WriteLine("Delete database succeeded");
                return true;
            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized."));
            }
        }
        #endregion

        #region Modifier's permissions
        public bool Edit(int id, string country, string city, short age, double salary, string payDay)
        {
            if (Thread.CurrentPrincipal.IsInRole("Edit"))
            {
                Console.WriteLine("Edit succeeded");
                return true;
            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized."));
            }
        }

        public bool Insert(string country, string city, short age, double salary, string payDay)
        {
            if (Thread.CurrentPrincipal.IsInRole("Insert"))
            {
                Console.WriteLine("Insert succeeded");
                return true;
            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized."));
            }
        }
        #endregion

        #region Viewer's permissions
        public String ViewAll()
        {
            if (Thread.CurrentPrincipal.IsInRole("View"))
            {
                Console.WriteLine("ViewAll() succeeded");
                return "ASD" + Environment.NewLine;
            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized."));
            }
        }

        public String ViewMaxPayed(bool tf)
        {
            if (Thread.CurrentPrincipal.IsInRole("View"))
            {
                Console.WriteLine("ViewMaxPayed() succeeded");
                return "ASD" + Environment.NewLine;
            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized."));
            }
        }

        public double AverageSalaryByCityAndAge(String city, short fromAge, short toAge)
        {
            if (Thread.CurrentPrincipal.IsInRole("View"))
            {
                Console.WriteLine("AverageSalaryByCityAndAge() succeeded");
                return 5.0;
            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized."));
            }
        }

        public double AverageSalaryByCountryAndPayday(String country, String payDay)
        {
            if (Thread.CurrentPrincipal.IsInRole("View"))
            {
                Console.WriteLine("AverageSalaryByCountryAndPayday() succeeded");
                return 5.0;
            }
            else
            {
                throw new FaultException<SecurityAccessDeniedException>(new SecurityAccessDeniedException(),
                    new FaultReason("Not authorized."));
            }
        }
        #endregion
    }
}
