using System;
using System.Net;

namespace devcon_installer.Utilities
{
    internal class FileDownloader
    {
        private readonly WebClient _downloadClient = new WebClient();

        public FileDownloader(string downloadUrl, string savePath)
        {
            DownloadUrl = downloadUrl;
            SavePath = savePath;

            _downloadClient.DownloadFileCompleted += (s, e) =>
            {
                OnDownloadCompleted?.Invoke(!e.Cancelled && e.Error == null);
            };
            _downloadClient.DownloadProgressChanged += (sender, args) =>
                OnProgressChanged?.Invoke(args.ProgressPercentage, "Downloading...");
        }

        public string DownloadUrl { get; }
        public string SavePath { get; }

        public event Action OnDownloadStarted;
        public event Action<bool> OnDownloadCompleted;
        public event Action<int, string> OnProgressChanged;

        public void StartDownload()
        {
            _downloadClient.DownloadFileAsync(new Uri(DownloadUrl), SavePath);
            OnDownloadStarted?.Invoke();
        }

        public void CancelDownload()
        {
            _downloadClient.CancelAsync();
        }
    }
}