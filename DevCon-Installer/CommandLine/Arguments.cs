using System.Collections.Generic;

namespace DevConInstaller.CommandLine
{
    public class Arguments: List<Argument>
    {
        public Dictionary<string, Argument> Lookup = new Dictionary<string, Argument>();

        public new void Add(Argument argument)
        {
            base.Add(argument);
            Lookup.Add(argument.Name, argument);
            if (argument.Children == null) return;
            foreach (var childArgument in argument.Children)
            {
                childArgument.Parent = argument;
                Add(childArgument);                
            }
        }
    }
}
