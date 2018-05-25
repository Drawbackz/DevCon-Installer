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
    public class MainPageViewModel : BaseViewModel
    {
        private readonly DevconInstaller _installer = new DevconInstaller();

        private DevconDownload _selectedDevconDownload;

        public MainPageViewModel()
        {
            _installer.OnLog += l =>
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
            _installer.OnSourcesUpdated += UpdateAvailableDownloads;
            UpdateAvailableDownloads();
        }

        public bool CanInstall => Progress == 0;

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

        public DevconDownload SelectedDevconDownload
        {
            get => _selectedDevconDownload;
            set
            {
                if (value == null)
                {
                    return;
                }
                var architectures = new List<SystemArchitecture>();
                foreach (var devconSource in value.Sources)
                    if (!architectures.Contains(devconSource.Architecture))
                        architectures.Add(devconSource.Architecture);
                AvailableArchitectures = new ObservableCollection<SystemArchitecture>(architectures);
                _selectedDevconDownload = value;
                if (AvailableArchitectures.Count > 0)
                    if (!AvailableArchitectures.Contains(SelectedArchitecture))
                        SelectedArchitecture = AvailableArchitectures[0];
            }
        }

        public ObservableCollection<DevconDownload> AvailableDownloads { get; private set; }
        public SystemArchitecture SelectedArchitecture { get; set; }
        public ObservableCollection<SystemArchitecture> AvailableArchitectures { get; set; }

        public int Progress { get; set; }
        public string ProgressText { get; set; } = string.Empty;
        public string StatusText { get; set; } = string.Empty;

        public int LogIndex { get; set; }
        public ObservableCollection<LogMessageBase> Log { get; set; } = new ObservableCollection<LogMessageBase>();

        public RelayCommand InstallCommand => new RelayCommand(() =>
        {
            _installer.Install(SelectedDevconDownload, SelectedArchitecture);
        });

        public RelayCommand UpdateCommand => new RelayCommand(() =>
        {
            _installer.UpdateSources();
        });

        public RelayCommand OpenDirectoryBrowser => new RelayCommand(() =>
        {
            var d = new FolderBrowserDialog();
            d.ShowDialog();
            if (Directory.Exists(d.SelectedPath))
                InstallDirectory = d.SelectedPath;
        });
        private void UpdateAvailableDownloads()
        {
            SelectedDevconDownload = null;

            var sources = DevconSources.ReadSaveFile();
            if (sources == null)
            {
                var dt = DateTime.Now.ToLongTimeString();
                Log.Add(new LogMessageError($"{dt}: There was an error processing the DevCon sources file"));
                Log.Add(new LogMessageError($"{dt}: Delete or repair devcon_sources.json and restart the application"));
                Log.Add(new LogMessage($"{dt}: Using default DevCon sources"));
                AvailableDownloads = new ObservableCollection<DevconDownload>(DevconSources.DefaultSources);
            }
            else
            {
                AvailableDownloads = new ObservableCollection<DevconDownload>(sources);
            }
            if(AvailableDownloads.Count > 0)
                SelectedDevconDownload = AvailableDownloads[0];
        }


    }
}