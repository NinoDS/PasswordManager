using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Security.Cryptography;

namespace PasswordManager
{
    static class Program
    {
        //CHAR PREFABS
        private static string digits = "1234567890";
        private static string lowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private static string upperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string specialChars = "~!@#$%^*-_=+[{]}/;:,.?";

        private static string _input;

        //COMMANDS
        private static Command<int> _generatePassword;
        private static Command _help;
        private static Command<string> _loadPassword;
        private static Command<string, string> _savePassword;
        private static List<object> _commandList;
        

        static void Main()
        {

            
            _generatePassword = new Command<int>("generate_password", "Generates password with the given length", "generate_password <password_length>",
                CreatePassword);
            _loadPassword = new Command<string>("load_password", "Load specific password based on user name",
                "load_password <username>", LoadPassword);
            _savePassword = new Command<string, string>("save_password", "Save password with username",
                "save_password <password>, <username>", SavePassword);
            _help = new Command("help", "Shows all possible commands", "help",
                ShowHelp);
            

            _commandList = new List<object>
            {
                _generatePassword,
                _loadPassword,
                _savePassword,
                _help
            };

            while (true)
            {
                _input = Console.ReadLine();
                HandleInput();
            }
            
        }

        private static void HandleInput()
        {
            string[] properties = _input.Split(" ");
            
            for (int i = 0; i < _commandList.Count; i++)
            {
                CommandBase commandBase = _commandList[i] as CommandBase;

                if (_input.Contains(commandBase?.CommandId ?? string.Empty))
                {

                    if (_commandList[i] is Command)
                    {
                        (_commandList[i] as Command)?.Invoke();
                    }
                    else if (_commandList[i] is Command<int>)
                    {
                        (_commandList[i] as Command<int>)?.Invoke(int.Parse(properties[1]));
                    }
                    else if (_commandList[i] is Command<string>)
                    {
                        (_commandList[i] as Command<string>)?.Invoke(properties[1]);
                    }
                    else if (_commandList[i] is Command<string, string>)
                    {
                        (_commandList[i] as Command<string, string>)?.Invoke(properties[1], properties[2]);
                    }
                }
 
            }
        }

        static void SavePassword(string password, string userName)
        {
            IO io = new IO();
            
            io.Write(password + "-" + userName, Directory.GetCurrentDirectory() + "\\password.txt", false);
        }

        static void LoadPassword(string _userName)
        {
            IO io = new IO();
            List<string> textList;
            string text = io.Read(Directory.GetCurrentDirectory() + "\\password.txt");
            text.Remove(text.Length - 1);
            string[] textArray = text.Split(" ");

            for (int i = 0; i < textArray.Length; i++)
            {
                if (textArray[i].Length > 0)
                {
                    string s = textArray[i];
                    string password = s.Split("-")[0];
                    string userName = s.Split("-")[1];
                    
                    if (userName == _userName)
                    {
                        Console.WriteLine(password);
                        return;
                    }
                }

            }
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

        private static void CheckPassword(string password)
        {
            int hasUpperCaseLetters = 0, hasLowerCaseLetters = 0, hasNumbers = 0, hasSymbols = 0;
            if (password.Length < 8)
            {
                Console.WriteLine("Password is unsafe!");
            }

            foreach (char i in password)
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

        static void ShowHelp()
        {
            for (int i = 0; i < _commandList.Count; i++)
            {
                CommandBase command = _commandList[i] as CommandBase;
                Console.WriteLine(command?.CommandFormat + " - " + command?.CommandDescription);
            }
        }
    }

}
