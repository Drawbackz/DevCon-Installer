namespace DevConInstaller.Core.Downloads
{
    public class DevconDownload : IDevconDownload
    {
        public string Name { get; set; }
        public DevconSource[] Sources { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}