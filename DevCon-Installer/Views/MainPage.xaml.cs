using System.Windows.Controls;
using DevConInstaller.ViewModels;

namespace DevConInstaller.Views
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainPageViewModel();
            LogListBox.SelectionChanged += LogListBox_SelectionChanged;
        }

        private void LogListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LogListBox.ScrollIntoView(LogListBox.SelectedItem);
        }
    }
}