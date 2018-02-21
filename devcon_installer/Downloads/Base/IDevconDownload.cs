namespace devcon_installer.Downloads.Base
{
    internal interface IDevconDownload
    {
        string Name { get; }
        DevconSource[] Sources { get; }
    }
}