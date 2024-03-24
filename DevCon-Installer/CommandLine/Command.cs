namespace DevConInstaller.CommandLine
{
    public abstract class Command
    {
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public Arguments Arguments { get; internal set; } = new Arguments();
    }
}
