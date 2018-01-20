namespace devcon_installer.Downloads.Base
{
    interface IDevconDownload
    {
        string Name { get; }
        DevconSource[] Sources { get; }
    }
}
