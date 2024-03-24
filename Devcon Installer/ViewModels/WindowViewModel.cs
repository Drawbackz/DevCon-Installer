using Devcon_Installer.ViewModels.Base;
using Devcon_Installer.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Devcon_Installer.ViewModels
{
    public class WindowViewModel : BaseViewModel
    {
        private readonly Window _window;

        private int _outerMarginSize = 10;
        private int _windowRadius;

        public WindowViewModel(Window window)
        {
            _window = window;
            _window.StateChanged += _window_StateChanged;

            MinimizeCommand = new RelayCommand(() => _window.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() =>
            {
                _window.WindowState = _window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            });
            CloseCommand = new RelayCommand(() => _window.Close());

            MenuCommand = new RelayCommand(() =>
                SystemCommands.ShowSystemMenu(_window, _window.PointToScreen(Mouse.GetPosition(_window))));
        }

        public Page Page { get; set; } = new MainPage();

        public bool Maximized { get; set; }

        public double WindowMinimumWidth { get; set; } = 600;
        public double WindowMinimumHeight { get; set; } = 350;

        public ICommand MinimizeCommand { get; set; }

        public ICommand MaximizeCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        public ICommand MenuCommand { get; set; }

        public bool TopMost { get; set; } = false;

        public int OuterMarginSize
        {
            get => _window.WindowState == WindowState.Maximized ? 0 : _outerMarginSize;
            set => _outerMarginSize = value;
        }

        public Thickness OuterMarginSizeThickness => new Thickness(OuterMarginSize);

        public int WindowRadius
        {
            get => _window.WindowState == WindowState.Maximized ? 0 : _windowRadius;
            set => _windowRadius = value;
        }

        public int InnerContentPadding { get; set; } = 4;
        public Thickness InnerContentPaddingThickness => new Thickness(InnerContentPadding + OuterMarginSize);

        public CornerRadius WindowCornerRadius => new CornerRadius(WindowRadius);

        public int ResizeBorder { get; set; } = 6;

        public Thickness ResizeBorderThickness => new Thickness(ResizeBorder + OuterMarginSize);

        public GridLength TitleHeightGridLength => new GridLength(TitleHeight + ResizeBorder);

        public int TitleHeight { get; set; } = 30;

        private void _window_StateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(ResizeBorderThickness));
            OnPropertyChanged(nameof(OuterMarginSize));
            OnPropertyChanged(nameof(OuterMarginSizeThickness));
            OnPropertyChanged(nameof(WindowRadius));
            OnPropertyChanged(nameof(WindowCornerRadius));
        }
    }
}