namespace DevConInstaller.Core.Downloads
{
    public interface IDevconSource
    {
        string Url { get; set; }
        string ExtractionName { get; set; }
        SystemArchitecture Architecture { get; set; }
    }
}