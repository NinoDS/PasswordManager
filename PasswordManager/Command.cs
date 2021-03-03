using System;

namespace PasswordManager
{
    public class CommandBase
    {
        public string CommandId { get; }

        public string CommandDescription { get; }

        public string CommandFormat { get; }

        protected CommandBase(string id, string description, string format)
        {
            CommandId = id;
            CommandDescription = description;
            CommandFormat = format;
        }
    }

    public class Command: CommandBase
    {
        private readonly Action _command;
        public Command(string id, string description, string format, Action command) : base(id, description, format)
        {
            this._command = command;
        }

        public void Invoke()
        {
            _command.Invoke();
        }
    }
    
    public class Command<T1>: CommandBase
    {
        private readonly Action<T1> _command;
        public Command(string id, string description, string format, Action<T1> command) : base(id, description, format)
        {
            this._command = command;
        }

        public void Invoke(T1 value)
        {
            _command.Invoke(value);
        }
    }
    
    public class Command<T1, T2>: CommandBase
    {
        private readonly Action<T1, T2> _command;
        public Command(string id, string description, string format, Action<T1, T2> command) : base(id, description, format)
        {
            this._command = command;
        }

        public void Invoke(T1 value0, T2 value1)
        {
            _command.Invoke(value0, value1);
        }
    }
    
    public class Command<T1, T2, T3>: CommandBase
    {
        private readonly Action<T1, T2, T3> _command;
        public Command(string id, string description, string format, Action<T1, T2, T3> command) : base(id, description, format)
        {
            this._command = command;
        }

        public void Invoke(T1 value0, T2 value1, T3 value2)
        {
            _command.Invoke(value0, value1, value2);
        }
    }
    
}