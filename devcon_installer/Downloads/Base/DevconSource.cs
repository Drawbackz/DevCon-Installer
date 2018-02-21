using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace devcon_installer.Downloads.Base
{
    public class DevconSource : IDevconSource
    {
        public string Sha256 { get; set; }
        public string Url { get; set; }
        public string ExtractionName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SystemArchitecture Architecture { get; set; }

        public override string ToString()
        {
            return Url;
        }
    }
}