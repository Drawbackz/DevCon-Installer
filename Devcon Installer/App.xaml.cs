using devcon_installer;
using devcon_installer.Downloads;
using devcon_installer.Downloads.Base;
using devcon_installer.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Devcon_Installer
{

    class DevConDownloadMatch
    {
        public DevconDownload Download { get; set; }
        public SystemArchitecture Architecture { get; set; }

        public DevConDownloadMatch(DevconDownload download, SystemArchitecture architecture)
        {
            Download = download;
            Architecture = architecture;
        }
    }

    public partial class App : Application
    {
        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string[] args = e.Args;
            if (args.Length > 0)
            {

                try
                {
                    DevconInstaller installer = new DevconInstaller();
                    ArgumentProcessor argumentProcessor = new ArgumentProcessor();

                    installer.OnLog += (log) =>
                    {
                        Console.WriteLine(log.Message);
                    };

                    DevConDownloadMatch getHashMatch(string hash)
                    {
                        DevconDownload[] sources = DevconSources.ReadSaveFile();
                        if(sources == null) { return null; }

                        var matchingDownloads = sources.Where(download => download.Sources.Any(source => source.Sha256 == hash));
                        if (matchingDownloads.Any())
                        {
                            DevconDownload download = matchingDownloads.First();
                            DevconSource source = download.Sources.FirstOrDefault(s => s.Sha256 == hash);
                            return new DevConDownloadMatch(download, source.Architecture);
                        }
                        else
                        {
                            return null;
                        }
                    }

                    async Task updateSources()
                    {
                        TaskCompletionSource<bool> updateComplete = new TaskCompletionSource<bool>();
                        installer.OnSourcesUpdated += () =>
                        {
                            updateComplete.TrySetResult(true);
                        };
                        installer.UpdateSources();
                        await updateComplete.Task;
                    }

                    async Task install()
                    {
                        TaskCompletionSource<bool> installTask = new TaskCompletionSource<bool>();

                        bool update = false;

                        installer.OnCompleted += (success) =>
                        {
                            installTask.TrySetResult(success);
                        };

                        string downloadHash = null;
                        argumentProcessor.AddArgument("-hash", (value) =>
                        {
                            downloadHash = value.ToUpper();
                        }, true);

                        argumentProcessor.AddArgument("-update", (value) =>
                        {
                            update = true;
                        });

                        argumentProcessor.AddArgument("-dir", (value) =>
                        {
                            var installDirectory = value;
                            if (!Directory.Exists(installDirectory))
                            {
                                throw new Exception($"Install Directory Does Not Exist\r\n{installDirectory}");
                            }
                            installer.InstallationDirectory = installDirectory;
                            installer.DownloadPath = installDirectory;
                        });

                        argumentProcessor.AddArgument("-addpath", (value) =>
                        {
                            if (!DevconInstaller.IsAdministrator())
                            {
                                throw new Exception("Add Path Requires Administrator Access");
                            }
                            installer.AddEnvironmentPath = true;
                        });

                        argumentProcessor.ProcessArguments(args.Skip(1).ToArray());
                        DevConDownloadMatch match = getHashMatch(downloadHash);
                        if(match == null && update)
                        {
                            FileInfo sourcesFileInfo = new FileInfo($"{Environment.CurrentDirectory}\\devcon_sources.json");
                            if (!sourcesFileInfo.Exists || DateTime.Now - sourcesFileInfo.LastWriteTime >= TimeSpan.FromHours(12))
                            {
                                await updateSources();
                            }
                            match = getHashMatch(downloadHash);
                        }
                        if(match == null)
                        {
                            throw new Exception($"Unable To Find Dowload {downloadHash}");
                        }
                        installer.Install(match.Download, match.Architecture);
                        await installTask.Task;
                    }


                    switch (args[0])
                    {
                        case "update":
                            await updateSources();
                            break;
                        case "install":
                            await install();
                            break;
                        default:
                            throw new ArgumentException("Unknown Command");
                    }
                    Current.Shutdown();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                    Current.Shutdown();
                }

            }
            else
            {
                StartupUri = new Uri("/Views/MainWindow.xaml", UriKind.Relative);
            }
        }
    }
}
