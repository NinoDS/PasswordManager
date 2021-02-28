using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager
{
    public class Cryptography
    {
        public static string IV = "1a1a1a1a1a1a1a1a";

        public string Encrypt(string decrypted, byte[] Key)
        {
            byte[] textBytes = Encoding.ASCII.GetBytes(decrypted);
            AesCryptoServiceProvider endec = new AesCryptoServiceProvider();
            endec.BlockSize = 128;
            endec.KeySize = 256;
            endec.IV = Encoding.ASCII.GetBytes(IV);
            endec.Key = Key;
            endec.Padding = PaddingMode.PKCS7;
            endec.Mode = CipherMode.CBC;
            ICryptoTransform iCryptoTransform = endec.CreateEncryptor(endec.Key, endec.IV);
            byte[] enc = iCryptoTransform.TransformFinalBlock(textBytes, 0, textBytes.Length);
            iCryptoTransform.Dispose();
            return Convert.ToBase64String(enc);
        }

        public string Decrypt(string encrypted, byte[] Key)
        {
            byte[] textbytes = Convert.FromBase64String(encrypted);
            AesCryptoServiceProvider endec = new AesCryptoServiceProvider();
            endec.BlockSize = 128;
            endec.KeySize = 256;
            endec.IV = Encoding.ASCII.GetBytes(IV);
            endec.Key = Key;
            endec.Padding = PaddingMode.PKCS7;
            endec.Mode = CipherMode.CBC;
            ICryptoTransform icrypt = endec.CreateDecryptor(endec.Key, endec.IV);
            byte[] enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
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