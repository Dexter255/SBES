using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [Serializable]
    public class Information
    {
        public static int count = 0;
        public int Id { get; set; }
        public String Drzava { get; set; }
        public String Grad { get; set; }
        public short Starost { get; set; }
        public double MesecnaPrimanja { get; set; }
        public String Year { get; set; }

        public Information()
        {
            Id = ++count;
        }

        public override string ToString()
        {
            return $"\nID: {Id}\nCountry: {Drzava}\nCity: {Grad}\nAge: {Starost}\nSalary: {MesecnaPrimanja}\nPayday: {Year}" + Environment.NewLine + Environment.NewLine;
        }
    }
}
