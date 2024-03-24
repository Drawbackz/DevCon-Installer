using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using DevConInstaller.Core.Downloads;
using DevConInstaller.Core.Logging;
using DevConInstaller.Core.Utilities;
using Newtonsoft.Json;

namespace DevConInstaller.Core
{
    public class DevconInstaller
    {
        private readonly CabExtractor _extractor = new CabExtractor();
        
        private FileDownloader _downloader;
        private FileDownloader Downloader
        {
            get => _downloader;
            set
            {
                if (_downloader != null)
                {
                    _downloader.OnProgressChanged -= OnProgressChanged;
                    _downloader.OnDownloadCompleted -= DownloaderOnCompleted;
                }
                _downloader = value;
                _downloader.OnProgressChanged += OnProgressChanged;
                _downloader.OnDownloadCompleted += DownloaderOnCompleted;
            }
        }

        private string _downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "devcon.cab");
        public string DownloadPath
        {
            get => _downloadPath;
            set => _downloadPath = Path.Combine(value, "devcon.cab");
        }

        private bool _addEnvironmentPath;
        public bool AddEnvironmentPath
        {
            get => _addEnvironmentPath;
            set
            {
                if (value && !Permissions.IsAdministrator()) return;
                _addEnvironmentPath = value;
            }
        }

        public string LastError { get; set; }


        public event Action<bool> OnCompleted;
        public event Action<int, string> OnProgressChanged;
        public event Action<LogMessageBase> OnLog;
        public event Action OnSourcesUpdated;

        public DevconSource DownloadSource { get; set; }

        public string InstallationDirectory { get; set; } =
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\DevCon";

        public void Install(DevconDownload download, SystemArchitecture architecture)
        {
            LastError = null;
            DownloadSource = download.Sources.First(dl => dl.Architecture == architecture);
            if (DownloadSource == null) throw new Exception("Download source does not support this architecture");

            Log("DevCon installation started");
            Downloader = new FileDownloader(DownloadSource.Url, DownloadPath);
            Downloader.StartDownload();
            Log("Download started");
        }

        public void UpdateSources()
        {
            LastError = null;

            var sourcesFile = $"{Environment.CurrentDirectory}\\devcon_sources.json";
            var backupFile = $"{sourcesFile}.bak";

            Log("Updating DevCon sources");
            if (File.Exists(sourcesFile))
            {
                if (File.Exists(backupFile))
                {
                    File.Delete(backupFile);
                }
                File.Move(sourcesFile, backupFile);
            }
            using (var wc = new WebClient())
            {
                wc.DownloadProgressChanged += (sender, args) =>
                {
                    OnProgressChanged?.Invoke(args.ProgressPercentage, "Downloading...");
                };
                wc.DownloadFileCompleted += (sender, args) =>
                {
                    if (args.Error != null)
                    {
                        File.Delete(sourcesFile);
                        if (File.Exists(backupFile))
                        {
                            File.Move(backupFile, sourcesFile);
                        }
                        else
                        {
                            File.WriteAllText(sourcesFile, JsonConvert.ToString(DevconSources.DefaultSources));
                        }
                        Log("Unable to download DevCon sources update", true);
                    }
                    else
                    {

                        File.Delete(backupFile);
                        Log("DevCon sources updated");
                        OnSourcesUpdated?.Invoke();
                    }
                    OnProgressChanged?.Invoke(0, string.Empty);
                };
                wc.DownloadFileAsync(new Uri("https://raw.githubusercontent.com/Drawbackz/DevCon-Installer/master/devcon_sources.json"), sourcesFile);
            }
        }

        public void Cancel()
        {
            Downloader.CancelDownload();
            Log("Installation cancelled", true);
        }

        private void DownloaderOnCompleted(bool success)
        {
            if (success)
            {
                Log("Download completed");
                if (ValidateCabDownload())
                    try
                    {
                        OnProgressChanged?.Invoke(50, "Extracting DevCon");
                        ExtractDevconCab();
                        File.Delete(Downloader.SavePath);
                        OnProgressChanged?.Invoke(100, "DevCon Extracted");
                        if (AddEnvironmentPath)
                        {
                            OnProgressChanged?.Invoke(50, "Registering path");
                            RegisterPath();
                            OnProgressChanged?.Invoke(100, "Path registered");
                        }
                    }
                    catch (Exception e)
                    {
                        LastError = e.Message;
                        Log(e.Message, true);
                    }
                else
                {
                    LastError = "Download file failed validation";
                }
            }
            else
            {
                LastError = "Download failed";
                Log("Download failed", true);
            }
            Log("Operation completed", LastError != null);
            OnProgressChanged?.Invoke(0, string.Empty);
            OnCompleted?.Invoke(LastError == null);
        }

        internal bool ValidateCabDownload()
        {
            Log("Verifying downloaded file");
            OnProgressChanged?.Invoke(50, "Validating...");
            if (!File.Exists(DownloadPath)) return false;
            var hash = DownloadSource.Sha256?.ToUpper();
            Log($"Expected hash: {hash}");
            var applicationHash = ChecksumTool.GetHashFromFile(DownloadPath, new SHA256Managed());
            Log($"Actual hash: {applicationHash}");
            OnProgressChanged?.Invoke(100, "Validation Completed");
            if (hash == applicationHash)
            {
                Log("File verification success");
                return true;
            }
            Log("File verification failed", true);
            return false;
        }

        private void ExtractDevconCab()
        {
            if (File.Exists(DownloadPath))
            {
                Log($"Extracting CAB file {DownloadSource.ExtractionName} from {DownloadPath}");
                _extractor.ExtractFile(DownloadPath, DownloadSource.ExtractionName, $"{InstallationDirectory}\\devcon.exe");
            }
            else
            {
                Log("CAB file was not found", true);
            }
        }

        private void RegisterPath()
        {
            Log("Registering DevCon to System PATH");
            try
            {
                const string name = "PATH";
                var pathString = Environment.GetEnvironmentVariable(name);
                var devconDirectory = Path.GetFullPath(InstallationDirectory);

                if (pathString == null) throw new Exception("Unable to get System PATH environment variable");
                if (pathString.Contains(devconDirectory)) return;

                var value = pathString + $";{devconDirectory}";
                const EnvironmentVariableTarget target = EnvironmentVariableTarget.Machine;
                Environment.SetEnvironmentVariable(name, value, target);
                Log("DevCon to System PATH success");
            }
            catch (Exception e)
            {
                Log(e.Message, true);
            }
        }

        private void Log(string message, bool isError = false)
        {
            message = $"{DateTime.Now.ToLongTimeString()}: {message}";
            var logItem = isError ? new LogMessageError(message) as LogMessageBase : new LogMessage(message);
            OnLog?.Invoke(logItem);
        }
    }
}