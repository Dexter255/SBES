using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class WCFService : IWCFService
    {
        public bool CreateDatabase(string filename)
        {
            Console.WriteLine("Create database succeeded");
            return true;
        }

        public bool DeleteDatabase(string filename)
        {
            Console.WriteLine("Delete database succeeded");
            return true;
        }

        public bool Edit(int id, string country, string city, short age, double salary, string payDay)
        {
            Console.WriteLine("Edit succeeded");
            return true;
        }

        public bool Insert(string country, string city, short age, double salary, string payDay)
        {
            Console.WriteLine("Insert succeeded");
            return true;
        }

        public String ViewAll()
        {
            Console.WriteLine("ViewAll() succeeded");
            return "ASD" + Environment.NewLine;
        }

        public String ViewMaxPayed(bool tf)
        {
            Console.WriteLine("ViewMaxPayed() succeeded");
            return "asd" + Environment.NewLine;
        }

        public double AverageSalaryByCityAndAge(String city, short fromAge, short toAge)
        {
            Console.WriteLine("AverageSalaryByCityAndAge() succeeded");
            return 5.00;
        }

        public double AverageSalaryByCountryAndPayday(String country, String payDay)
        {
            Console.WriteLine("AverageSalaryByCountryAndPayday() succeeded");
            return 1.00;
        }
    }
}
