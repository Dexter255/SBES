using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Service
{


    public class WCFDatabase : IWCFService
    {
        public Dictionary<String, Dictionary<int, Information>> DbList = new Dictionary<string, Dictionary<int, Information>>();
        public String DatabaseNames = "spisakBaza.txt";

        public WCFDatabase()
        {
            DeserializeData();
        }


        #region Serialization
        public void SerializeData()
        {
            //podesavamo serializer za informacije (kako bismo sacuvali sve postojece nazive baza)
            XmlSerializer serializerDbNames = new XmlSerializer(typeof(List<String>));
            StringWriter swDbNames = new StringWriter();

            //pomocna promenljiva u koju cemo smestati sve informacije u tekucoj bazi
            List<Information> tempdataitems;
            //pomocna promenljiva koja cuva nazive svih baza kako bismo ih sacuvali
            List<String> databaseNames = new List<String>();

            //prolazimo kroz sve baze u sistemu
            foreach (KeyValuePair<String, Dictionary<int, Information>>kvp in DbList)
            {
                //podesavamo serializer za informacije (podatke unutar svake baze)
                XmlSerializer serializer = new XmlSerializer(typeof(List<Information>));
                StringWriter sw = new StringWriter();

                //dodajemo nazive baza u pomocnu listu da ih sacuvamo u fajlu
                databaseNames.Add(kvp.Key);
                
                //za svaku bazu, napunimo je njenim podacima i to snimimo u fajl nakon fora
                tempdataitems = new List<Information>();

                foreach(KeyValuePair<int, Information> kvp1 in kvp.Value)
                {
                    tempdataitems.Add(kvp1.Value);  //dodajemo u pomocnu listu kako bismo to upisali u fajl
                }
                serializer.Serialize(sw, tempdataitems);
                //upisujemo u svaku bazu informacije koje su vezane za njih
                File.Delete(kvp.Key);
                File.AppendAllText(kvp.Key, sw.ToString());

            }
            //pravimo fajl sa svim nazivima baza podataka
            serializerDbNames.Serialize(swDbNames, databaseNames);
            File.Delete(DatabaseNames);
            File.AppendAllText(DatabaseNames, swDbNames.ToString());

        }

        public void DeserializeData()
        {
            if (!File.Exists(DatabaseNames))
            {
                return;
            }

            String xml = File.ReadAllText(DatabaseNames);   //sve baze podataka


            XmlSerializer xs = new XmlSerializer(typeof(List<String>));
            StringReader sr = new StringReader(xml);
            List<String> templist = (List<String>)xs.Deserialize(sr);

            foreach(var dbName in templist)
            {
                String xmlDb = File.ReadAllText(dbName);

                XmlSerializer xsDb = new XmlSerializer(typeof(List<Information>));
                StringReader srDb = new StringReader(xmlDb);
                List<Information> templistDb = (List<Information>)xsDb.Deserialize(srDb);

                Dictionary<int, Information> currentDb = new Dictionary<int, Information>();
                foreach(var info in templistDb)
                {
                    currentDb.Add(info.Id, info);
                }
                //kada smo procitali sve iz tekuce baze to dodajemo u DbList
                DbList.Add(dbName, currentDb);
            }

        }
        #endregion


        public bool CreateDatabase(string databaseName)
        {
            if (!DbList.ContainsKey(databaseName))
            {
                DbList.Add(databaseName, new Dictionary<int, Information>());
                //SerializeData();
                return true;
            }
            return false;
        }

        public bool DeleteDatabase(string databaseName)
        {
            if (DbList.ContainsKey(databaseName))
            {
                DbList.Remove(databaseName);
                return true;
            }
            return false;
        }

        public bool Edit(string databaseName, int id, string country, string city, short age, double salary, string payDay)
        {
            if(DbList.ContainsKey(databaseName) && DbList[databaseName].ContainsKey(id))
            {
                DbList[databaseName][id].Drzava = country;
                DbList[databaseName][id].Grad = city;
                DbList[databaseName][id].Starost = age;
                DbList[databaseName][id].MesecnaPrimanja = salary;
                DbList[databaseName][id].Year = payDay;
                //SerializeData();
                return true;
            }

            return false;
        }

        public bool Insert(string databaseName, string country, string city, short age, double salary, string payDay)
        {
            if (DbList.ContainsKey(databaseName))
            {
                Information info = new Information() { Drzava = country, Grad = city, Starost = age, MesecnaPrimanja = salary, Year = payDay };
                DbList[databaseName].Add(info.Id, info);
                //SerializeData();
                return true;
            }

            return false;
        }

        public string ViewAll(string databaseName)
        {
            throw new NotImplementedException();
        }

        public string ViewMaxPayed(string databaseName)
        {
            throw new NotImplementedException();
        }

        public double AverageSalaryByCityAndAge(string databaseName, string city, short fromAge, short toAge)
        {
            throw new NotImplementedException();
        }

        public double AverageSalaryByCountryAndPayday(string databaseName, string country, string payDay)
        {
            throw new NotImplementedException();
        }
    }
}
