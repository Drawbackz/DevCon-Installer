using System.Windows.Controls;
using Devcon_Installer.ViewModels;

namespace Devcon_Installer.Views
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
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
