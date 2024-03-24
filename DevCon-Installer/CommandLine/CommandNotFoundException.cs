using System;

namespace DevConInstaller.CommandLine
{
    public class CommandNotFoundException : Exception
    {
        public CommandNotFoundException() { }

        public CommandNotFoundException(string message) : base(message) { }

        public CommandNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}