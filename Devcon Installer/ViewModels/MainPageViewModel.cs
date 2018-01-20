using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using devcon_installer;
using devcon_installer.Downloads;
using devcon_installer.Logging;
using Devcon_Installer.ViewModels.Base;

namespace Devcon_Installer.ViewModels
{
    public class MainPageViewModel:BaseViewModel
    {
        public bool CanInstall
        {
            get { return Progress == 0; }
        }

        private readonly DevconInstaller _installer = new DevconInstaller();

        public string InstallDirectory
        {
            get => _installer.InstallationDirectory;
            set => _installer.InstallationDirectory = value;
        }
        public bool AddToPath
        {
            get => _installer.AddEnvironmentPath;
            set
            {
                if (!DevconInstaller.IsAdministrator())
                {
                    MessageBox.Show("Must run as administrator");
                    return;
                }
                _installer.AddEnvironmentPath = value;
            }
        }

        private DevconDownload _selectedDevconDownload;
        public DevconDownload SelectedDevconDownload
        {
            get { return _selectedDevconDownload; }
            set
            {
                var architectures = new List<SystemArchitecture>();
                foreach (var devconSource in value.Sources)
                {
                    if (!architectures.Contains(devconSource.Architecture))
                    {
                        architectures.Add(devconSource.Architecture);
                    }
                }
                AvailableArchitectures = new ObservableCollection<SystemArchitecture>(architectures);
                _selectedDevconDownload = value;
                if (AvailableArchitectures.Count > 0)
                {
                    if (!AvailableArchitectures.Contains(SelectedArchitecture))
                    {
                        SelectedArchitecture = AvailableArchitectures[0];
                    }
                }
            }
        }

        public ObservableCollection<DevconDownload> AvailableDownloads { get; private set; }
        public SystemArchitecture SelectedArchitecture { get; set; }
        public ObservableCollection<SystemArchitecture> AvailableArchitectures { get; set; }

        public int Progress { get; set; } = 0;
        public string ProgressText { get; set; } = string.Empty;
        public string StatusText { get; set; } = string.Empty;

        public int LogIndex { get; set; }
        public ObservableCollection<LogMessageBase> Log { get; set; } = new ObservableCollection<LogMessageBase>();

        public RelayCommand InstallCommand => new RelayCommand(() =>
        {
            _installer.Install(SelectedDevconDownload, SelectedArchitecture);
        });
        public RelayCommand OpenDirectoryBrowser => new RelayCommand(() =>
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            d.ShowDialog();
            if (Directory.Exists(d.SelectedPath))
            {
                InstallDirectory = d.SelectedPath;
            }
        });

        public MainPageViewModel()
        {
            _installer.OnLog += (l) =>
            {
                Log.Add(l);
                LogIndex = Log.Count - 1;
            };
            _installer.OnProgressChanged += (i, s) =>
            {
                Progress = i;
                ProgressText = i == 0 ? string.Empty : i + "%";
                StatusText = s;
            };
            UpdateAvailableDownloads();
        }

        private void UpdateAvailableDownloads()
        {
            AvailableDownloads = new ObservableCollection<DevconDownload>(DevconSources.ReadSaveFile());
            if (AvailableDownloads.Count > 0)
            {
                SelectedDevconDownload = AvailableDownloads[0];
            }
        }
    }
}
