using System.IO;

namespace PasswordManager
{
    public class IO
    {
        public void Write(string text, string path, bool isOverwriting)
        {
            StreamWriter sw = new StreamWriter(path, !isOverwriting);
            sw.Write(text + " ");
            sw.Close();
        }

        public string Read(string path)
        {
            StreamReader sr = new StreamReader(path);
            string text = sr.ReadToEnd();
            sr.Close();
            return text;
        }

    }

    
}