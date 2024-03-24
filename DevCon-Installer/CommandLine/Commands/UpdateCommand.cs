namespace DevConInstaller.CommandLine.Commands
{
    public class UpdateCommand : Command
    {
        public UpdateCommand()
        {
            Name = "update";
            Description = "Update the DevCon sources file";
            Arguments = new Arguments();
        }
    }
}
