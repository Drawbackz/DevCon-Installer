using System;
using System.IO;
using devcon_installer.Downloads;
using devcon_installer.Downloads.Base;
using Newtonsoft.Json;

namespace devcon_installer
{
    public static class DevconSources
    {
        private static readonly DevconDownload[] DefaultSources =
        {
            new DevconDownload
            {
                Name = "Windows 10 version 1709 (Fall Creators Update)",
                Sources = new[]
                {
                    new DevconSource
                    {
                        Url =
                            "https://download.microsoft.com/download/7/D/D/7DD48DE6-8BDA-47C0-854A-539A800FAA90/wdk/Installers/82c1721cd310c73968861674ffc209c9.cab",
                        Architecture = SystemArchitecture.X86,
                        ExtractionName = "fil5a9177f816435063f779ebbbd2c1a1d2",
                        Sha256 = "633B8149E480BB52CCFE7CBEDFFE13F30FBC528F5F6EECC03ACBE9C7E24E8CA2"
                    },
                    new DevconSource
                    {
                        Url =
                            "https://download.microsoft.com/download/7/D/D/7DD48DE6-8BDA-47C0-854A-539A800FAA90/wdk/Installers/787bee96dbd26371076b37b13c405890.cab",
                        Architecture = SystemArchitecture.X64,
                        ExtractionName = "filbad6e2cce5ebc45a401e19c613d0a28f",
                        Sha256 = "A38E409617FC89D0BA1224C31E42AF4344013FEA046D2248E4B9E03F67D5908A"
                    }
                }
            },
            new DevconDownload
            {
                Name = "Windows 10 version 1703 (Creators Update)",
                Sources = new[]
                {
                    new DevconSource
                    {
                        Url =
                            "https://download.microsoft.com/download/4/E/0/4E07EAAD-E394-4EA8-B2B8-D46E46A409C5/wdk/Installers/82c1721cd310c73968861674ffc209c9.cab",
                        Architecture = SystemArchitecture.X86,
                        ExtractionName = "fil5a9177f816435063f779ebbbd2c1a1d2",
                        Sha256 = "4CA8946960846052661E11D4FCDD21BA9B855C7AED67322189AF6F68BB9CE940"
                    },
                    new DevconSource
                    {
                        Url =
                            "https://download.microsoft.com/download/7/D/D/7DD48DE6-8BDA-47C0-854A-539A800FAA90/wdk/Installers/787bee96dbd26371076b37b13c405890.cab",
                        Architecture = SystemArchitecture.X64,
                        ExtractionName = "filbad6e2cce5ebc45a401e19c613d0a28f",
                        Sha256 = "A38E409617FC89D0BA1224C31E42AF4344013FEA046D2248E4B9E03F67D5908A"
                    }
                }
            },
            new DevconDownload
            {
                Name = "Windows 10 version 1511 (November Update)",
                Sources = new[]
                {
                    new DevconSource
                    {
                        Url =
                            "https://download.microsoft.com/download/C/E/1/CE19C726-6036-4443-845B-A652B0F48CD7/wdk/Installers/82c1721cd310c73968861674ffc209c9.cab",
                        Architecture = SystemArchitecture.X86,
                        ExtractionName = "fil5a9177f816435063f779ebbbd2c1a1d2",
                        Sha256 = "494D08720A0FD72328BEA9021E305B42AA60D0D3AB53ECBB3840967F30D9D303"
                    },
                    new DevconSource
                    {
                        Url =
                            "https://download.microsoft.com/download/C/E/1/CE19C726-6036-4443-845B-A652B0F48CD7/wdk/Installers/787bee96dbd26371076b37b13c405890.cab",
                        Architecture = SystemArchitecture.X64,
                        ExtractionName = "filbad6e2cce5ebc45a401e19c613d0a28f",
                        Sha256 = "4B87DE87E4E39B413C305BC5C910F406808AF661F9E8691E0050B0722E5803D2"
                    }
                }
            },
            new DevconDownload
            {
                Name = "Windows 10 version 1507",
                Sources = new[]
                {
                    new DevconSource
                    {
                        Url =
                            "https://download.microsoft.com/download/1/2/1/121AA3A0-6E03-4852-B7E6-39C741E1E942/wdk/Installers/82c1721cd310c73968861674ffc209c9.cab",
                        Architecture = SystemArchitecture.X86,
                        ExtractionName = "fil5a9177f816435063f779ebbbd2c1a1d2",
                        Sha256 = "F8B55D0F2BB29C86688AF6BDE84A09914D792297E02ED7CA824354C1DC39A00E"
                    },
                    new DevconSource
                    {
                        Url =
                            "https://download.microsoft.com/download/1/2/1/121AA3A0-6E03-4852-B7E6-39C741E1E942/wdk/Installers/787bee96dbd26371076b37b13c405890.cab",
                        Architecture = SystemArchitecture.X64,
                        ExtractionName = "filbad6e2cce5ebc45a401e19c613d0a28f",
                        Sha256 = "98AFEA2438714064D08556F2D8B8D48A9A6F6DF8C7613CBD4C6E44A9D4D3189F"
                    }
                }
            },
            new DevconDownload
            {
                Name = "Windows 10 Insider Preview",
                Sources = new[]
                {
                    new DevconSource
                    {
                        Url =
                            "https://download.microsoft.com/download/D/B/5/DB538B89-04A0-4D74-AD65-E721D93FFD08/wdk/Installers/82c1721cd310c73968861674ffc209c9.cab",
                        Architecture = SystemArchitecture.X86,
                        ExtractionName = "fil5a9177f816435063f779ebbbd2c1a1d2",
                        Sha256 = "0A91474AE0F19A8CE8048E9551D195DF30F9478CD26A963B8189247B9A2C5878"
                    },
                    new DevconSource
                    {
                        Url =
                            "https://download.microsoft.com/download/D/B/5/DB538B89-04A0-4D74-AD65-E721D93FFD08/wdk/Installers/787bee96dbd26371076b37b13c405890.cab",
                        Architecture = SystemArchitecture.X64,
                        ExtractionName = "filbad6e2cce5ebc45a401e19c613d0a28f",
                        Sha256 = "0A91474AE0F19A8CE8048E9551D195DF30F9478CD26A963B8189247B9A2C5878"
                    }
                }
            },
            new DevconDownload
            {
                Name = "Windows 8.1 Update",
                Sources = new[]
                {
                    new DevconSource
                    {
                        Url =
                            "https://download.microsoft.com/download/0/8/C/08C7497F-8551-4054-97DE-60C0E510D97A/wdk/Installers/af0d6547860d8f68c1b0c9da530f699d.cab",
                        Architecture = SystemArchitecture.X86,
                        ExtractionName = "fil5a9177f816435063f779ebbbd2c1a1d2",
                        Sha256 = "0D70458C91466B1D18B73FCEF6A7E688E1655A03D42D72D69C0047276B66724B"
                    },
                    new DevconSource
                    {
                        Url =
                            "https://download.microsoft.com/download/0/8/C/08C7497F-8551-4054-97DE-60C0E510D97A/wdk/Installers/09844d1815314132979ed88093f49c6f.cab",
                        Architecture = SystemArchitecture.X64,
                        ExtractionName = "filbad6e2cce5ebc45a401e19c613d0a28f",
                        Sha256 = "2783EAFBF69012E9F680C9F8BD4AD083F745EEE0C75FDC63C84AF064C89A0878"
                    }
                }
            },
            new DevconDownload
            {
                Name = "Windows 8",
                Sources = new[]
                {
                    new DevconSource
                    {
                        Url =
                            "https://download.microsoft.com/download/2/4/C/24CA7FB3-FF2E-4DB5-BA52-62A4399A4601/wdk/Installers/af0d6547860d8f68c1b0c9da530f699d.cab",
                        Architecture = SystemArchitecture.X86,
                        ExtractionName = "fil5a9177f816435063f779ebbbd2c1a1d2",
                        Sha256 = "8E41D3479C25DC69FD5B4B5464A9981D5EC3C47EA4571CCD6ECD354A6B601FDD"
                    },
                    new DevconSource
                    {
                        Url =
                            "https://download.microsoft.com/download/2/4/C/24CA7FB3-FF2E-4DB5-BA52-62A4399A4601/wdk/Installers/09844d1815314132979ed88093f49c6f.cab",
                        Architecture = SystemArchitecture.X64,
                        ExtractionName = "filbad6e2cce5ebc45a401e19c613d0a28f",
                        Sha256 = "7892464A6DAEAF5D3FE7B500B1EDC9F25E9E0D513E20B991B1B6C8D0479E7138"
                    }
                }
            },
            new DevconDownload
            {
                Name = "Windows XP/Vista + Windows Server 2003",
                Sources = new[]
                {
                    new DevconSource
                    {
                        Url =
                            "https://download.microsoft.com/download/6/e/4/6e481b67-54af-4340-a534-25de4229cfc6/support.cab",
                        Architecture = SystemArchitecture.X86,
                        ExtractionName = "devcon.exe",
                        Sha256 = "ADAED2F9BCD668800AC4A593535DDF8BD43617A408825CBE9A6044045CC183AB"
                    }
                }
            }
        };

        public static string SavePath { get; set; }

        public static DevconDownload[] ReadSaveFile()
        {
            var filePath = SavePath ?? $"{Environment.CurrentDirectory}\\devcon_sources.json";
            if (!File.Exists(filePath)) CreateSourcesFile(filePath);
            return JsonConvert.DeserializeObject<DevconDownload[]>(File.ReadAllText(filePath));
        }

        private static void CreateSourcesFile(string savePath)
        {
            File.WriteAllText(savePath, JsonConvert.SerializeObject(DefaultSources, Formatting.Indented));
        }
    }
}