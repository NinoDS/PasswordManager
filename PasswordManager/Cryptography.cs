using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager
{
    public class Cryptography
    {
        public static string IV = "1b6dee1435f21219";

        public string Encrypt(string decrypted, byte[] key)
        {
            byte[] textBytes = Encoding.ASCII.GetBytes(decrypted);
            AesCryptoServiceProvider endec = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = 256,
                IV = Encoding.ASCII.GetBytes(IV),
                Key = key,
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC
            };
            ICryptoTransform iCryptoTransform = endec.CreateEncryptor(endec.Key, endec.IV);
            byte[] enc = iCryptoTransform.TransformFinalBlock(textBytes, 0, textBytes.Length);
            iCryptoTransform.Dispose();
            return Convert.ToBase64String(enc);
        }

        public string Decrypt(string encrypted, byte[] key)
        {
            byte[] textBytes = Convert.FromBase64String(encrypted);
            AesCryptoServiceProvider endec = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = 256,
                IV = Encoding.ASCII.GetBytes(IV),
                Key = key,
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC
            };
            ICryptoTransform icrypt = endec.CreateDecryptor(endec.Key, endec.IV);
            byte[] enc = icrypt.TransformFinalBlock(textBytes, 0, textBytes.Length);
            icrypt.Dispose();
            return Encoding.ASCII.GetString(enc);
        }

        public byte[] CreateHash(string text)
        {
            HashAlgorithm hashAlgorithm = new SHA256Managed();
            byte[] hash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
            return hash;
        }
    }
}