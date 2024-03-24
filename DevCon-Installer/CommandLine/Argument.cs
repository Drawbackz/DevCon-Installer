using System;

namespace DevConInstaller.CommandLine
{
    public class Argument
    {
        public string Name { get; }
        public bool Required { get; }
        public string Description { get; }

        public Argument Parent { get; set; }
        public Argument[] Children { get; set; } = Array.Empty<Argument>();

        internal Action<string> SetValue { get; set; }

        public Argument(string name, string description, Action<string> valueHandler, bool required = false)
        {
            Name = name;
            Required = required;
            Description = description;
            SetValue = valueHandler;
        }
    }

}
