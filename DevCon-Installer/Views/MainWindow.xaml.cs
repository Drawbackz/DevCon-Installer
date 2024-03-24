using System;
using System.Windows;
using System.Windows.Forms;
using DevConInstaller.ViewModels;

namespace DevConInstaller.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new WindowViewModel(this);
            StateChanged += MainWindow_StateChanged;
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState != WindowState.Maximized) return;
            var screen = Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            var workingArea = screen.WorkingArea;
            MaxWidth = workingArea.Width + 5;
            MaxHeight = workingArea.Height + 5;
        }
    }
}