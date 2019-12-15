using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class DataCryptography
    {
        public static byte[] EncryptData(X509Certificate2 certificate, String message)
        {

            AesCryptoServiceProvider cryptoProvider = new AesCryptoServiceProvider();
            cryptoProvider.Padding = PaddingMode.Zeros;
            cryptoProvider.Key = AesCryptoServiceProvider.Create().Key;
            cryptoProvider.Mode = CipherMode.CBC;
            cryptoProvider.GenerateIV();

            MemoryStream ms = new MemoryStream();
            ICryptoTransform aesEncrypt = cryptoProvider.CreateEncryptor();

            CryptoStream cryptoStream;

            byte[] keyPlusIV = new byte[cryptoProvider.Key.Length + cryptoProvider.IV.Length];

            keyPlusIV = cryptoProvider.Key.Concat(cryptoProvider.IV).ToArray();

            RSACryptoServiceProvider rsaCSP = certificate.PublicKey.Key as RSACryptoServiceProvider;
            byte[] keyPlusIVEncrypted = new byte[256];
            keyPlusIVEncrypted = rsaCSP.Encrypt(keyPlusIV, true);
            byte[] messageEncrypted;
            using (cryptoStream = new CryptoStream(ms, aesEncrypt, CryptoStreamMode.Write))
            {
                cryptoStream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                cryptoStream.FlushFinalBlock();
                messageEncrypted = ms.ToArray();

            }

            byte[] fullEncryptedMessage = keyPlusIVEncrypted.Concat(messageEncrypted).ToArray();

            String toReturn = Encoding.ASCII.GetString(fullEncryptedMessage);

            return fullEncryptedMessage;

        }
        public static String DecryptData(X509Certificate2 certificate, byte[] recieved)
        {
            AesCryptoServiceProvider cryptoProvider = new AesCryptoServiceProvider();
            cryptoProvider.Padding = PaddingMode.Zeros;
            cryptoProvider.Mode = CipherMode.CBC;

            //byte[] fullCriptedByteMessage = Encoding.ASCII.GetBytes(recieved);
            byte[] fullCriptedByteMessage = recieved;

            //smallPortion = largeBytes.Take(4).ToArray();
            //largeBytes = largeBytes.Skip(4).Take(5).ToArray();

            byte[] keyPlusIv = fullCriptedByteMessage.Take(256).ToArray();

            RSACryptoServiceProvider rsaDECR = certificate.PrivateKey as RSACryptoServiceProvider;
            byte[] decryptedKeyPlusIV = rsaDECR.Decrypt(keyPlusIv, true);

            //sve sto nam treba u bajtima
            byte[] key = decryptedKeyPlusIV.Take(32).ToArray();
            byte[] iv = decryptedKeyPlusIV.Skip(32).Take(decryptedKeyPlusIV.Length - 32).ToArray();
            byte[] message = fullCriptedByteMessage.Skip(256).Take(recieved.Length - 256).ToArray();


            cryptoProvider.Key = key;
            cryptoProvider.IV = iv;


            CryptoStream cryptoStream;
            MemoryStream ms = new MemoryStream();
            ICryptoTransform aesDecrypt = cryptoProvider.CreateDecryptor();


            using (cryptoStream = new CryptoStream(ms, aesDecrypt, CryptoStreamMode.Write))
            {
                cryptoStream.Write(message, 0, message.Length);
                //cryptoStream.FlushFinalBlock();
                return Encoding.ASCII.GetString(ms.ToArray());
            }
        }
    }
}
