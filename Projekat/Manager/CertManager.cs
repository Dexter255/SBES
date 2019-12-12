using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace Manager
{
    public class CertManager
    {
        /// <summary>
        /// Get a certificate with the specified subject name from the predefined certificate storage
        /// Only valid certificates should be considered
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="storeLocation"></param>
        /// <param name="subjectName"></param>
        /// <returns> The requested certificate. If no valid certificate is found, returns null. </returns>
        public static X509Certificate2 GetCertificateFromStorage(StoreName storeName, StoreLocation storeLocation, string subjectName)
        {
            X509Certificate2 certificate = null;
            var certificateStore = new X509Store(storeName, storeLocation);

            certificateStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            var certificateCollection = certificateStore.Certificates.Find(X509FindType.FindBySubjectName, subjectName, false);

            certificateStore.Close();
            if (certificateCollection[0] != null)
            {
                certificate = certificateCollection[0];
            }
            return certificate;
        }
        
    }
}
