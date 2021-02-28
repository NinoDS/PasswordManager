using System.IO;

namespace PasswordManager
{
    public class IO
    {
        public void Write(string text, string path, bool isOverwriting)
        {
            StreamWriter sw = new StreamWriter(path, !isOverwriting);
            sw.Write(text);
            sw.Close();
        }

        public string Read(string path)
        {
            StreamReader sr = new StreamReader(path);
            string text = sr.ReadToEnd();
            sr.Close();
            return text;
        }

        public byte[] ReadBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public void WriteBytes(byte[] bytes, string path)
        {
            File.WriteAllBytes(path, bytes);
        }
        
        public string ReadEncrypted(string path, byte[] key)
        {
            Cryptography decrypter = new Cryptography();
            return decrypter.Decrypt(Read(path), key);
        }
        
        public void WriteEncrypted(string text ,string path, byte[] key)
        {
            Cryptography cryptography = new Cryptography();

            Write(File.Exists(path) ? cryptography.Encrypt(cryptography.Decrypt(Read(path), key)  + text + " ", key) : cryptography.Encrypt(text + " ", key) , path, true);
             
        }


    }

    
}