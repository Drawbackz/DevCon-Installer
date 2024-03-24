using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DevConInstaller.CommandLine.Commands;
using DevConInstaller.Core;
using DevConInstaller.Core.Downloads;

namespace DevConInstaller.CommandLine
{

    internal class ConsoleApp
    {

        public static async Task Main(string[] args)
        {
            var availableCommands = new Command[]
            {
                new UpdateCommand(),
                new InstallCommand()
            };

            var installer = new DevconInstaller();
            installer.OnLog += (log) =>
            {
                Console.WriteLine(log.Message);
            };

            try
            {
                var command = CommandProcessor.ProcessCommands(availableCommands, args);

                switch (command)
                {
                    case InstallCommand install:

                        if (!install.UseLatest && string.IsNullOrEmpty(install.Hash))
                        {
                            throw new ArgumentException("-latest or -hash required");
                        }
                        
                        var sources = DevconSources.ReadSaveFile() ?? DevconSources.DefaultSources;

                        DevConDownloadMatch download;
                        if (install.UseLatest)
                        {
                            if (install.Update)
                            {
                                await UpdateSources(installer);
                                sources = DevconSources.ReadSaveFile();
                            }
                            if (sources == null)
                            {
                                throw new FileNotFoundException("Sources file is missing");
                            }
                            download = GetLatestDevConVersion(sources, install.Architecture);
                        }
                        else
                        {
                            download = GetHashDevConVersion(sources, install.Hash);
                            if (download == null && install.Update)
                            {
                                await UpdateSources(installer);
                                sources = DevconSources.ReadSaveFile();
                            }
                            download = GetHashDevConVersion(sources, install.Hash);
                        }

                        if (download == null)
                        {
                            throw new Exception("Unable to find requested DevCon version in sources");
                        }
                        if (install.Directory != null)
                        {
                            if (!Directory.Exists(install.Directory))
                            {
                                throw new Exception($"Install Directory Does Not Exist\r\n{install.Directory}");
                            }
                            installer.DownloadPath = install.Directory;
                            installer.InstallationDirectory = install.Directory;
                        }
                        installer.AddEnvironmentPath = install.AddToPath;
                        await InstallDevCon(installer, download);
                        break;

                    case UpdateCommand update:
                        await UpdateSources(installer, true);
                        break;
                }

            }
            catch (ArgumentException ex)
            {
                Logger.Error(ex.Message);
                CommandProcessor.PrintCommandDetails(availableCommands);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        private static DevConDownloadMatch GetHashDevConVersion(DevconDownload[] sources, string hash)
        {
            if (sources == null) { return null; }

            var matchingDownloads = sources.Where(download => download.Sources.Any(source => source.Sha256 == hash)).ToArray();
            if (matchingDownloads.Any())
            {
                var download = matchingDownloads.First();
                var match = download.Sources.FirstOrDefault(source => source.Sha256 == hash);
                if (match != null) return new DevConDownloadMatch(download, match.Architecture);
            }
            return null;
        }

        private static DevConDownloadMatch GetLatestDevConVersion(DevconDownload[] sources, SystemArchitecture architecture)
        {
            var download = sources.First();
            var match = download.Sources.FirstOrDefault(source => source.Architecture == architecture);
            if (match != null) return new DevConDownloadMatch(download, match.Architecture);
            return null;
        }

        private static Task InstallDevCon(DevconInstaller installer, DevConDownloadMatch matchedFile)
        {
            var installComplete = new TaskCompletionSource<bool>();
            installer.OnCompleted += (success) =>
            {
                installComplete.TrySetResult(true);
            };
            installer.Install(matchedFile.Download, matchedFile.Architecture);
            return installComplete.Task;
        }

        private static Task UpdateSources(DevconInstaller installer, bool force = false)
        {
            if (!force && DateTime.Now - DevconSources.LastUpdate() < TimeSpan.FromHours(12))
            {
                Logger.Warning("Skipping update - Last update less than 12 hours ago");
                return Task.CompletedTask;
            }
            
            var updateComplete = new TaskCompletionSource<bool>();
            installer.OnSourcesUpdated += () =>
            {
                updateComplete.TrySetResult(true);
            };
            installer.UpdateSources();
            return updateComplete.Task;
        }
    }

}