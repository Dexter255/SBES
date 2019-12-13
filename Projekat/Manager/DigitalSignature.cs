using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Contracts;

namespace Manager
{
    public class DigitalSignature
    {
        public static byte[] Create(string message, X509Certificate2 certificate)
        {
            // entity se hash-uje SHA1 algoritmom
            SHA1Managed sha1 = new SHA1Managed();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(message));

            RSACryptoServiceProvider csp = certificate.PrivateKey as RSACryptoServiceProvider;
            byte[] signature = csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));

            return signature;
        }


        public static bool Verify(string message, byte[] signature, X509Certificate2 certificate)
        {
            SHA1Managed sha1 = new SHA1Managed();

            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(message));

            RSACryptoServiceProvider csp = certificate.PublicKey.Key as RSACryptoServiceProvider;

            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);
        }
    }
}
