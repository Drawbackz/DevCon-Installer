using System;
using System.Windows.Input;

namespace Devcon_Installer.ViewModels.Base
{
    public class RelayCommand : ICommand
    {
        private readonly Action _action;

        public RelayCommand(Action action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}