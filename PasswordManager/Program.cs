using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        private static string _passwordPath;
        private static string _checkHashPath;
        private static byte[] _checkHash;

        //COMMANDS
        private static Command _changePassword;
        private static Command<int> _generatePassword;
        private static Command _help;
        private static Command<string> _loadPassword;
        private static Command<string, string, string> _savePassword;
        private static Command<string> _checkPassword;
        private static Command _load_all;
        
        private static List<object> _commandList;
        

        static void Main()
        {
            Directory.CreateDirectory($"C:\\Users\\{Environment.UserName}\\AppData\\Roaming\\PasswordManager");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;

            Cryptography cryptography = new Cryptography();
            IO io = new IO();
            _checkHashPath = $"C:\\Users\\{Environment.UserName}\\AppData\\Roaming\\PasswordManager\\checkhash";
            if (!File.Exists(_checkHashPath))
            {
                Console.WriteLine("Enter master password!");
                io.WriteBytes(cryptography.CreateHash($"Ich bin {Console.ReadLine()}, der große König. - Und ich Diogenes, der Hund."), _checkHashPath); 
                Console.WriteLine("Master password set");
                Console.Clear();
            }

            _checkHash = io.ReadBytes(_checkHashPath);

            _passwordPath = $"C:\\Users\\{Environment.UserName}\\AppData\\Roaming\\PasswordManager\\password";

            _load_all = new Command("load_all", "Loads all passwords", "load_password", LoadAllPasswords);
            _changePassword = new Command("change_password", "Change master password", "change_password",
                ChangePassword);
            _checkPassword = new Command<string>("check_password", "Checks if a password is safe", "check_password <password>", CheckPassword);
            _generatePassword = new Command<int>("generate_password", "Generates password with the given length", "generate_password <password_length>",
                CreatePassword);
            _loadPassword = new Command<string>("load_password", "Load specific password based on user name or website",
                "load_password <username or website>", LoadPassword);
            _savePassword = new Command<string, string, string>("save_password", "Save password with username",
                "save_password <password>, <username>", SavePassword);
            _help = new Command("help", "Shows all possible commands", "help",
                ShowHelp);
            

            _commandList = new List<object>
            {
                _load_all,
                _changePassword,
                _generatePassword,
                _loadPassword,
                _savePassword,
                _help,
                _checkPassword
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
                    else if (_commandList[i] is Command<string, string, string>)
                    {
                        (_commandList[i] as Command<string, string, string>)?.Invoke(properties[1], properties[2], properties[3]);
                    }
                }
 
            }
        }

        static void SavePassword(string password, string userName, string website)
        {
            Console.WriteLine("Saving...");
            Console.WriteLine("Input master password");
            Cryptography cryptography = new Cryptography();
            string input = Console.ReadLine();
            
            if (!cryptography.CreateHash($"Ich bin {input}, der große König. - Und ich Diogenes, der Hund.").SequenceEqual(_checkHash))
            {
                Console.WriteLine("Wrong Password");
                return;
            }
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            if (input != null)
            {
                Console.WriteLine(new string('*', input.Length));

                IO io = new IO();

                io.WriteEncrypted(password + "-" + userName + "-" + website, _passwordPath,
                    cryptography.CreateHash(input));
            }
        }

        static void LoadAllPasswords()
        {
            Console.WriteLine("Input master password");
            Cryptography cryptography = new Cryptography();
            string input = Console.ReadLine();
            
            if (!cryptography.CreateHash($"Ich bin {input}, der große König. - Und ich Diogenes, der Hund.").SequenceEqual(_checkHash))
            {
                Console.WriteLine("Wrong Password");
                return;
            }

            PasswordReplacer pr1 = new PasswordReplacer();
            if (input != null)
            {
                pr1.ReplacePassword(Console.CursorTop - 1, 0, 0, input.Length);

                IO io = new IO();
                string text = io.ReadEncrypted(_passwordPath, cryptography.CreateHash(input));
                text.Remove(text.Length - 1);
                string[] textArray = text.Split(" ");

                foreach (var t in textArray)
                {
                    if (t.Length > 0)
                    {
                        string s = t;
                        string password = s.Split("-")[0];
                        string userName = s.Split("-")[1];
                        string website = s.Split("-")[2];
                        
                        PasswordReplacer pr2 = new PasswordReplacer();
                        Console.WriteLine($"Username: \"{userName}\", Website: \"{website}\" Password: \"{password}\"");

                        pr2.ReplacePassword(Console.CursorTop - 1, 37 + userName.Length + website.Length, 10000, password.Length);

                    }
                }
            }
        }

        static void LoadPassword(string userNameOrWebsite)
        {
            Console.WriteLine("Input master password");
            Cryptography cryptography = new Cryptography();
            string input = Console.ReadLine();
            
            if (!cryptography.CreateHash($"Ich bin {input}, der große König. - Und ich Diogenes, der Hund.").SequenceEqual(_checkHash))
            {
                Console.WriteLine("Wrong Password");
                return;
            }

            PasswordReplacer pr1 = new PasswordReplacer();
            if (input != null)
            {
                pr1.ReplacePassword(Console.CursorTop - 1, 0, 0, input.Length);

                IO io = new IO();
                string text = io.ReadEncrypted(_passwordPath, cryptography.CreateHash(input));
                text.Remove(text.Length - 1);
                string[] textArray = text.Split(" ");

                foreach (var t in textArray)
                {
                    if (t.Length > 0)
                    {
                        string s = t;
                        string password = s.Split("-")[0];
                        string userName = s.Split("-")[1];
                        string website = s.Split("-")[2];

                        if (userName == userNameOrWebsite || website == userNameOrWebsite)
                        {
                            PasswordReplacer pr2 = new PasswordReplacer();
                            Console.WriteLine($"Website: \"{website}\", Password: \"{password}\"");

                            pr2.ReplacePassword(Console.CursorTop - 1, website.Length + 2, 10000, password.Length);
                            return;
                        }
                    }
                }
            }
        }

        static void ChangePassword()
        {
            Console.WriteLine("Input old master password");
            Cryptography cryptography = new Cryptography();
            string input = Console.ReadLine();
            
            if (!cryptography.CreateHash($"Ich bin {input}, der große König. - Und ich Diogenes, der Hund.").SequenceEqual(_checkHash))
            {
                Console.WriteLine("Wrong Password");
                return;
            }

            IO io = new IO();
            
            Console.WriteLine("Input new master password");
            string newPassword = Console.ReadLine();
            
            io.WriteBytes(cryptography.CreateHash($"Ich bin {newPassword}, der große König. - Und ich Diogenes, der Hund."), _checkHashPath); 
            
            
            byte[] oldKey = cryptography.CreateHash(input);
            byte[] newKey = cryptography.CreateHash(newPassword);
            

            if (File.Exists(_passwordPath))
            {
                string content = io.ReadEncrypted(_passwordPath, oldKey);
                io.Write(cryptography.Encrypt(content, newKey), _passwordPath, true);
            }
            _checkHash = io.ReadBytes(_checkHashPath);
            Console.WriteLine("Master password set");
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
            Console.WriteLine("Checking password...");
            int hasUpperCaseLetters = 0, hasLowerCaseLetters = 0, hasNumbers = 0, hasSymbols = 0;
            if (password.Length < 8)
            {
                Console.WriteLine("Password is unsafe!");
                return;
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

    class PasswordReplacer
    {
        public async void ReplacePassword(int curserLine, int lineOffset, int delay, int passwordlength)
        {
            await Task.Delay(delay);
            Console.SetCursorPosition(lineOffset, curserLine);
            Console.WriteLine(new string('*', passwordlength));
        }

    }

}
