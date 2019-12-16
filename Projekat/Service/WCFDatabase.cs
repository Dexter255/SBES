using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Service
{
    public class WCFDatabase : IWCFService
    {
        public Dictionary<String, Dictionary<int, Information>> DbList = new Dictionary<string, Dictionary<int, Information>>();
        public String DatabaseNames = "spisakBaza.txt";
        private static WCFDatabase instance = null;

        private WCFDatabase()
        {
            DeserializeData();
        }

        public static WCFDatabase InitializeDb()
        {
            if(instance == null)
            {
                instance = new WCFDatabase();
            }
            return instance;
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

        #region Admin's operations
        public string CreateDatabase(string databaseName)
        {
            if (!DbList.ContainsKey(databaseName))
            {
                DbList.Add(databaseName, new Dictionary<int, Information>());
                //SerializeData();
                return $"Database with name '{databaseName}' successfully created.\n";
            }
            return $"Database with name '{databaseName}' already exists.\n";
        }

        public string DeleteDatabase(string databaseName)
        {
            if (DbList.ContainsKey(databaseName))
            {
                DbList.Remove(databaseName);
                return $"Database with name '{databaseName}' successfully deleted.\n";
            }
            return $"Database with name '{databaseName}' doesn't exists.\n";
        }
        #endregion  

        #region Modifier's operations
        public string Edit(string message, byte[] signature)
        {
            //Debugger.Launch();
            // CN=userModifier, OU=Modifiers; 9755AE1E112121811EE2DC67B7CF696CB7F69727
            string clientName = (Thread.CurrentPrincipal.Identity as GenericIdentity).Name.Split(',', ';')[0].Split('=')[1];

            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, clientName);

            if (DigitalSignature.Verify(message, signature, certificate))
            {
                // message = $"{databaseName}:{country}:{city}:{age}:{salary}:{payday}:{id}"
                // parts[0] - databaseName
                // parts[1] - country
                // parts[2] - city
                // parts[3] - age
                // parts[4] - salary
                // parts[5] - payday
                // parts[6] - id
                string[] parts = message.Split(':');
                int id = Int32.Parse(parts[6]);

                if (DbList.ContainsKey(parts[0]))
                {
                    if (DbList[parts[0]].ContainsKey(id))
                    {
                        DbList[parts[0]][id].Drzava = parts[1].Trim().ToLower();
                        DbList[parts[0]][id].Grad = parts[2].Trim().ToLower();
                        DbList[parts[0]][id].Starost = short.Parse(parts[3]);
                        DbList[parts[0]][id].MesecnaPrimanja = Double.Parse(parts[4]);
                        DbList[parts[0]][id].Year = parts[5].Trim();
                        //SerializeData();
                        return $"Existing entity with id '{id}' in database '{parts[0]}' successfully edited.\n";
                    }
                    else
                    {
                        return $"Entity with id '{id}' in database '{parts[0]}' doesn't exists.\n";
                    }
                }
                else
                {
                    return $"Database with name '{parts[0]}' doesn't exists.\n";
                }
            }

            return $"Message was changed by interceptor.\n";
        }

        public string Insert(string message, byte[] signature)
        {
            // CN=userModifier, OU=Modifiers; 9755AE1E112121811EE2DC67B7CF696CB7F69727
            string clientName = (Thread.CurrentPrincipal.Identity as GenericIdentity).Name.Split(',', ';')[0].Split('=')[1];

            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, clientName);

            if (DigitalSignature.Verify(message, signature, certificate))
            {
                // message = $"{databaseName}:{country}:{city}:{age}:{salary}:{payday}"
                // parts[0] - databaseName
                // parts[1] - country
                // parts[2] - city
                // parts[3] - age
                // parts[4] - salary
                // parts[5] - payday
                string[] parts = message.Split(':');
                int id = Int32.Parse(parts[6]);

                if (DbList.ContainsKey(parts[0]))
                {
                    Information info = new Information() { Drzava = parts[1].Trim().ToLower(), Grad = parts[2].Trim().ToLower(), Starost = short.Parse(parts[3]), MesecnaPrimanja = Double.Parse(parts[4]), Year = parts[5].Trim() };
                    DbList[parts[0]].Add(info.Id, info);
                    return $"New entity successfully inserted in database '{parts[0]}'.\n";
                }
                else
                {
                    return $"Database with name '{parts[0]}' doesn't exists.\n";
                }
            }

            return $"Message was changed by interceptor.\n";
        }
        #endregion

        #region Viewer's operations
        public byte[] ViewAll(string databaseName)
        {
            string message = "----------------------------------------------------\nAll entities:\n\n";
            string clientName = (Thread.CurrentPrincipal.Identity as GenericIdentity).Name.Split(',', ';')[0].Split('=')[1];
            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, clientName);

            if (DbList.ContainsKey(databaseName))
            {
                foreach (Information information in DbList[databaseName].Values)
                {
                    message += information.ToString();
                }

                message += "----------------------------------------------------\n";
                return DataCryptography.EncryptData(certificate, message);
            }
            else
            {
                return DataCryptography.EncryptData(certificate, $"Database with name '{databaseName}' doesn't exists.\n");
            }
        }

        public byte[] ViewMaxPayed(string databaseName)
        {

            string clientName = (Thread.CurrentPrincipal.Identity as GenericIdentity).Name.Split(',', ';')[0].Split('=')[1];
            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, clientName);


            //Debugger.Launch();
            Dictionary<string, List<Information>> informations = new Dictionary<string, List<Information>>();
            string message = "----------------------------------------------------\nMax salary from all states:\n\n";

            if (DbList.ContainsKey(databaseName))
            {
                foreach (Information information in DbList[databaseName].Values)
                {
                    if (informations.ContainsKey(information.Drzava))
                    {
                        informations[information.Drzava].Add(information);
                    }
                    else
                    {
                        informations.Add(information.Drzava, new List<Information>() { information });
                    }
                }

                foreach(var pair in informations)
                {
                    message += $"{pair.Key}:\t{pair.Value.Max(x => x.MesecnaPrimanja).ToString()}\n";
                }
            }
            else
            {
                return DataCryptography.EncryptData(certificate, $"Database with name '{databaseName}' doesn't exists.\n");
            }

            return DataCryptography.EncryptData(certificate, message + "\n----------------------------------------------------\n");
        }

        public byte[] AverageSalaryByCityAndAge(string databaseName, string city, short fromAge, short toAge)
        {

            string clientName = (Thread.CurrentPrincipal.Identity as GenericIdentity).Name.Split(',', ';')[0].Split('=')[1];
            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, clientName);

            Dictionary<string, List<Information>> informations = new Dictionary<string, List<Information>>();
            string message = "----------------------------------------------------\nAvarage salary by city and age:\n\n";

            if (DbList.ContainsKey(databaseName))
            {
                foreach(Information information in DbList[databaseName].Values)
                {
                    if(information.Grad == city.Trim().ToLower() && information.Starost >= fromAge & information.Starost <= toAge)
                    {
                        if (informations.ContainsKey(information.Grad))
                        {
                            informations[information.Grad].Add(information);
                        }
                        else
                        {
                            informations.Add(information.Grad, new List<Information>() { information });
                        }
                    }
                }

                foreach(var pair in informations)
                {
                    message += $"{pair.Key}:\t{pair.Value.Average(x => x.MesecnaPrimanja).ToString()}\n";
                }
            }
            else
            {
                return DataCryptography.EncryptData(certificate, $"Database with name '{databaseName}' doesn't exists.\n");
            }

            return DataCryptography.EncryptData(certificate, message + "\n----------------------------------------------------\n");
        }

        public byte[] AverageSalaryByCountryAndPayday(string databaseName, string country, string payDay)
        {

            string clientName = (Thread.CurrentPrincipal.Identity as GenericIdentity).Name.Split(',', ';')[0].Split('=')[1];
            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, clientName);

            Dictionary<string, List<Information>> informations = new Dictionary<string, List<Information>>();
            string message = "----------------------------------------------------\nAvarage salary by country and payday:\n\n";

            if (DbList.ContainsKey(databaseName))
            {
                foreach (Information information in DbList[databaseName].Values)
                {
                    if (information.Drzava == country.Trim().ToLower() && information.Year == payDay.Trim())
                    {
                        if (informations.ContainsKey(information.Drzava))
                        {
                            informations[information.Drzava].Add(information);
                        }
                        else
                        {
                            informations.Add(information.Drzava, new List<Information>() { information });
                        }
                    }
                }

                foreach (var pair in informations)
                {
                    message += $"{pair.Key}:\t{pair.Value.Average(x => x.MesecnaPrimanja).ToString()}\n";
                }
            }
            else
            {
                return DataCryptography.EncryptData(certificate, $"Database with name '{databaseName}' doesn't exists.\n");
            }

            return DataCryptography.EncryptData(certificate, message + "\n----------------------------------------------------\n");
        }

        public byte[] ViewDatabasesNames()
        {

            string clientName = (Thread.CurrentPrincipal.Identity as GenericIdentity).Name.Split(',', ';')[0].Split('=')[1];
            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, clientName);

            string message = "----------------------------------------------------\nDatabases names:\n\n";

            foreach(string databaseName in DbList.Keys)
            {
                message += $"{databaseName}\n";
            }

            return DataCryptography.EncryptData(certificate, message + "\n----------------------------------------------------\n");
        }
        #endregion
    }
}
