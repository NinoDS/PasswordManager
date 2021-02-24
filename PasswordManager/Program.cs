using System;
using System.Collections.Generic;
using System.IO.Compression;

namespace PasswordManager
{
    class Program
    {
        private static string asciiChars = "!\"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
        
        static void Main(string[] args)
        {
            Interpreter(Console.ReadLine());
        }

        static void Interpreter(string text)
        {
            string[] textArray = text.Split(" ");
            int arguments = textArray.Length - 2;
            if (textArray.Length == 0)
                return;

            if (textArray[0] == "pm")
            {
                if (arguments == 1)
                {
                    if (textArray[1] == "password")
                    {
                        
                        Random rnd = new Random();
                        int passwordLenth = Int32.Parse(textArray[2]);
                        string password = "";
                        for (int i = 0; i < passwordLenth; i++)
                        {
                            password += asciiChars[rnd.Next(asciiChars.Length)];
                        }
                        Console.WriteLine(password);
                    }
                }
            }
        }
    }
}
