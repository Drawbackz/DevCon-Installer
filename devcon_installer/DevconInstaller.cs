using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Principal;
using devcon_installer.Downloads;
using devcon_installer.Downloads.Base;
using devcon_installer.Logging;
using devcon_installer.Utilities;
using Newtonsoft.Json;

namespace devcon_installer
{
    public class DevconInstaller
    {
        private readonly CabExtractor _extractor;

        private bool _addEnvironmentPath;

        private FileDownloader _downloader;

        public DevconInstaller()
        {
            _extractor = new CabExtractor();
        }

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

        public DevconSource DownloadSource { get; set; }

        public string LastError { get; set; }

        public string DownloadPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "devcon.cab");

        public bool AddEnvironmentPath
        {
            get => _addEnvironmentPath;
            set
            {
                if (value)
                    if (!IsAdministrator())
                        return;
                _addEnvironmentPath = value;
            }
        }

        public string InstallationDirectory { get; set; } =
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\DevCon";

        public event Action<bool> OnCompleted;
        public event Action<int, string> OnProgressChanged;
        public event Action<LogMessageBase> OnLog;
        public event Action OnSourcesUpdated;

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public void Install(DevconDownload download, SystemArchitecture architecture)
        {
            LastError = null;
            DownloadSource = download.Sources.First(dl => dl.Architecture == architecture);
            if (DownloadSource == null) throw new Exception("Download source does not suppport this achitecture");

            Log("DevCon installation started");
            Downloader = new FileDownloader(DownloadSource.Url, DownloadPath);
            Downloader.StartDownload();
            Log("Download started");
        }

        public void UpdateSources()
        {
            Log("Updating DevCon sources...");
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
                        Log("Unable to download DevCon sources update", true);
                    }
                    else
                    {
                        OnSourcesUpdated?.Invoke();
                        Log("DevCon sources updated");
                    }
                    OnProgressChanged?.Invoke(0, string.Empty);
                };
                wc.DownloadFileAsync(new Uri("https://raw.githubusercontent.com/Drawbackz/DevCon-Sources/master/devcon_sources.json"), $"{Environment.CurrentDirectory}\\devcon_sources.json");
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
                        OnProgressChanged?.Invoke(50, "Extracting...");
                        ExtractDevconCab();
                        File.Delete(Downloader.SavePath);
                        OnProgressChanged?.Invoke(100, "Extracted.");
                        if (AddEnvironmentPath)
                        {
                            OnProgressChanged?.Invoke(50, "Registering path...");
                            RegisterPath();
                            OnProgressChanged?.Invoke(100, "Path registered.");
                        }
                    }
                    catch (Exception e)
                    {
                        LastError = e.Message;
                        Log(e.Message, true);
                    }
                else
                    LastError = "Download file failed validation";
            }
            else
            {
                LastError = "Download failed";
                Log("Download failed", true);
            }
            OnCompleted?.Invoke(LastError == null);
            Log("Operation completed", LastError != null);
            OnProgressChanged?.Invoke(0, string.Empty);
        }

        internal bool ValidateCabDownload()
        {
            Log("Verifying downloaded file...");
            OnProgressChanged?.Invoke(50, "Validating...");
            if (!File.Exists(DownloadPath)) return false;
            var hash = DownloadSource.Sha256;
            Log($"Expected hash: {hash}");
            var applicationHash = ChecksumTool.GetHashFromFile(DownloadPath, Algorithms.SHA256);
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
                _extractor.ExtractFile(DownloadPath, DownloadSource.ExtractionName,
                    $"{InstallationDirectory}\\devcon.exe");
            }
            else
            {
                Log("CAB file was not found", true);
            }
        }

        private void RegisterPath()
        {
            Log("Registering DevCon to system PATH");
            try
            {
                const string name = "PATH";
                var pathString = Environment.GetEnvironmentVariable(name);

                if (pathString == null) throw new Exception("Unable to get path data");
                if (pathString.Contains(InstallationDirectory)) return;

                var value = pathString + $";{InstallationDirectory}";
                const EnvironmentVariableTarget target = EnvironmentVariableTarget.Machine;
                Environment.SetEnvironmentVariable(name, value, target);
                Log("DevCon to system PATH success");
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