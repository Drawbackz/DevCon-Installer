using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevConInstaller.CommandLine
{
    public class CommandProcessor: List<Command>
    {
        public CommandProcessor() { }
        public CommandProcessor(IEnumerable<Command> commands)
        {
            AddRange(commands);
        }

        public static void PrintCommandDetails(IEnumerable<Command> commands)
        {
            var bars = CreateBars(4);
            Logger.Log(bars + " AVAILABLE COMMANDS " + bars);

            foreach (var command in commands)
            {
                Logger.Log(" ");
                Logger.Log($"Command: {command.Name}");
                Logger.Log($"Description: {command.Description}");
                if(command.Arguments.Count > 0)
                {
                    Logger.Warning("Arguments:");
                    LogArguments(command.Arguments);
                }
            }

            Logger.Log();
            Logger.Log(bars + "====================" + bars);
            Logger.Log();
        }

        public static Command ProcessCommands(Command[] supportedCommands, string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("No arguments provided");
            };

            var command = supportedCommands.First(cmd => cmd.Name == args[0]);
            if (command != null)
            {
                var processedArgs = ProcessArguments(command, args.Skip(1).ToArray());
                var missingArgs = GetMissingRequiredArguments(processedArgs);
                if (!missingArgs.Any()) return command;

                var errorMessage = new StringBuilder();
                errorMessage.AppendLine("Missing required argument(s):");
                foreach (var arg in missingArgs)
                {
                    errorMessage.AppendLine();
                    errorMessage.AppendLine(arg.Name);
                    errorMessage.AppendLine(arg.Description);
                    errorMessage.AppendLine();
                }

                throw new ArgumentException(errorMessage.ToString());
            }

            throw new CommandNotFoundException($"Unknown Command: {args[0]}");
        }

        private static IEnumerable<Argument> GetRequiredArguments(Argument[] arguments)
        {
            var requiredArguments = new List<Argument>();
            foreach (var argument in arguments)
            {
                if (argument.Required)
                {
                    requiredArguments.Add(argument);
                }
                requiredArguments.AddRange(GetRequiredArguments(argument.Children));
            }

            return requiredArguments.ToArray();
        }
        
        private static Argument[] GetMissingRequiredArguments(Argument[] processedArgs)
        {
            var missingArguments = new List<Argument>();
            var requiredArguments = GetRequiredArguments(processedArgs);
            
            foreach(var argument in requiredArguments)
            {
                if (argument.Required && !processedArgs.Contains(argument))
                {
                    missingArguments.Add(argument);
                }
            }

            return missingArguments.ToArray();
        }

        private static Argument[] ProcessArguments(Command command, IReadOnlyList<string> args)
        {
            var processedArgs = new Dictionary<string, Argument>();

            for (var i = 0; i < args.Count; i++)
            {
                var arg = args[i];
                if (command.Arguments.Lookup.TryGetValue(arg, out var argument))
                {
                    processedArgs.Add(arg, argument);
                    if (i + 1 >= args.Count)
                    {
                        argument.SetValue(null);
                        continue;
                    };
                    var hasValue = !args[i + 1].StartsWith("--");
                    var value = hasValue ? args[i + 1] : null;
                    argument.SetValue(value);
                    if (hasValue) i++;
                }
                else
                {
                    throw new ArgumentException($"Unknown argument: {arg}");
                }
            }

            return processedArgs.Values.ToArray();
        }

        private static string CreateBars(int divisor = 1)
        {
            if (divisor == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            return new string('=', Console.WindowWidth / divisor);
        }

        private static void LogArguments(IEnumerable<Argument> arguments)
        {
            if (arguments == null) { return;}
            foreach (var argument in arguments)
            {
                Logger.Warning($"{argument.Name}: {argument.Description} ");
                Logger.Log();
            }
        }
    }


}
