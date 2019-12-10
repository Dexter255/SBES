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

        public bool View()
        {
            Console.WriteLine("View1 succeeded");
            return true;
        }

        public bool View(bool tf)
        {
            Console.WriteLine("View2 succeeded");
            return true;
        }

        public bool View(string city, short fromAge, short toAge)
        {
            Console.WriteLine("View3 succeeded");
            return true;
        }

        public bool View(string country, string payDay)
        {
            Console.WriteLine("View4 succeeded");
            return true;
        }
    }
}
