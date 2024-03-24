using System;
using System.Windows;
using System.Runtime.InteropServices;
using DevConInstaller.CommandLine;

namespace DevConInstaller
{   
    public partial class App
    {
        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();
        
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (e.Args.Length > 0)
                try
                {
                    await ConsoleApp.Main(e.Args);
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.ToString());
                    Environment.Exit(1);
                }
            else
            {
                FreeConsole();
                StartupUri = new Uri("/Views/MainWindow.xaml", UriKind.Relative);
            }
        }
    }
}