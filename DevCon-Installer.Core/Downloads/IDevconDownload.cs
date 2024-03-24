namespace DevConInstaller.Core.Downloads
{
    internal interface IDevconDownload
    {
        string Name { get; }
        DevconSource[] Sources { get; }
    }
}