using System;
using System.Collections.Generic;
using System.IO.Compression;

namespace PasswordManager
{
    class Program
    {
        private static string digits = "1234567890";
        private static string lowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private static string upperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string specialChars = "~!@#$%^*-_=+[{]}/;:,.?";
        public static Command GENERATE_PASSWORD;
        public static List<object> commandList;
        public static string input;
        
        static void Main(string[] args)
        {
            input = Console.ReadLine();
            GENERATE_PASSWORD = new Command("generate_password", "Generates a safe password", "generate_password",
                () => CreatePassword(10));

            commandList = new List<object>
            {
                GENERATE_PASSWORD
            };
            
            HandleInput();
        }

        public static void HandleInput()
        {
            for (int i = 0; i < commandList.Count; i++)
            {
                CommandBase commandBase = commandList[i] as CommandBase;

                if (input.Contains(commandBase.CommandId))
                {

                    if (commandList[i] as Command != null)
                    {
                        (commandList[i] as Command).Invoke();
                    }
                    
                }
            }
        }

        static void Interpreter(string text)
        {
            
        }

        static void CreatePassword(int passwordLength)
        {
            Random rnd = new Random();
            string password = "";
            for (int i = 0; i < passwordLength; i++)
            {
                password += (digits + lowerCaseLetters + upperCaseLetters + specialChars)[rnd.Next((digits + lowerCaseLetters + upperCaseLetters + specialChars).Length)];
            }
            Console.WriteLine(password);
        }

        static void CheckPassword(string password)
        {
            int hasUpperCaseLetters = 0, hasLowerCaseLetters = 0, hasNumbers = 0, hasSymbols = 0;
            string chars = digits + lowerCaseLetters + upperCaseLetters + specialChars;
            if (password.Length < 8)
            {
                Console.WriteLine("Password is unsafe!");
            }

            foreach (var i in password)
            {
                if (digits.Contains(i))
                {
                    hasNumbers = 1;
                }
                else if (lowerCaseLetters.Contains(i))
                {
                    hasLowerCaseLetters = 1;
                }
                else if (upperCaseLetters.Contains(i))
                {
                    hasUpperCaseLetters = 1;
                }
                else if (specialChars.Contains(i))
                {
                    hasSymbols = 1;
                }
            }

            Console.WriteLine(hasNumbers + hasLowerCaseLetters + hasUpperCaseLetters + hasSymbols >= 3
                ? "Password is safe!"
                : "Password is unsafe!");
        }
    }

}
