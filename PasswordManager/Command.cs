using System;

namespace PasswordManager
{
    public class CommandBase
    {
        private string _commandId;
        private string _commandDescription;
        private string _commandFormat;

        public string CommandId => _commandId;

        public string CommandDescription => _commandDescription;

        public string CommandFormat => _commandFormat;

        public CommandBase(string id, string description, string format)
        {
            _commandId = id;
            _commandDescription = description;
            _commandFormat = format;
        }
    }

    public class Command: CommandBase
    {
        private Action command;
        public Command(string id, string description, string format, Action command) : base(id, description, format)
        {
            this.command = command;
        }

        public void Invoke()
        {
            command.Invoke();
        }
    }
}