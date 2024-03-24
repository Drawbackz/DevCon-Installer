using DevConInstaller.Core;
using DevConInstaller.Core.Downloads;

namespace DevConInstaller.CommandLine
{
    internal class DevConDownloadMatch
    {
        public DevconDownload Download { get; set; }
        public SystemArchitecture Architecture { get; set; }

        public DevConDownloadMatch(DevconDownload download, SystemArchitecture architecture)
        {
            Download = download;
            Architecture = architecture;
        }
    }
}
