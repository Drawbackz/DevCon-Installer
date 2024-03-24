using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace devcon_installer.Utilities
{
    internal class Argument
    {
        public string Name { get; }
        public bool Required { get; }
        public string Value { get; private set; }
        public Action<string> Action { get; }

        public Argument(string name, Action<string> action, bool required = false)
        {
            Name = name;
            Action = action;
            Required = required;
        }

        public void SetValue(string value)
        {
            Value = value;
        }
    }

    public class ArgumentProcessor
    {
        private readonly Dictionary<string, Argument> _actions = new Dictionary<string, Argument>();

        public void AddArgument(string argName, Action<string> action, bool required = false)
        {
            _actions[argName] = new Argument(argName, action, required);
        }

        public void ProcessArguments(string[] args)
        {
            var missingRequiredArgs = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (_actions.TryGetValue(arg, out Argument argument))
                {
                    if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                    {
                        argument.SetValue(args[i + 1]);
                        i++;
                    }
                    argument.Action(argument.Value);
                }
                else
                {
                    throw new ArgumentException($"Unknown argument: {arg}");
                }
            }

            foreach (var action in _actions.Values)
            {
                if (action.Required && action.Value == null)
                {
                    missingRequiredArgs.Add(action.Name);
                }
            }

            if (missingRequiredArgs.Any())
            {
                StringBuilder errorMessage = new StringBuilder();
                errorMessage.AppendLine("Missing required arguments:");
                foreach (var arg in missingRequiredArgs)
                {
                    errorMessage.AppendLine(arg);
                }
                throw new ArgumentException(errorMessage.ToString());
            }
        }
    }


}
