namespace devcon_installer.Downloads.Base
{
    public interface IDevconSource
    {
        string Url { get; set; }
        string ExtractionName { get; set; }
        SystemArchitecture Architecture { get; set; }
    }
}
