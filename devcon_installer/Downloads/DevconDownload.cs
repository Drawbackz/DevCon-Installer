using devcon_installer.Downloads.Base;

namespace devcon_installer.Downloads
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