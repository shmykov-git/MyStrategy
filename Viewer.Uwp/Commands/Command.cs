using System;
using System.Windows.Input;

namespace Viewer.Uwp.Commands
{
    class Command : ICommand
    {
        private readonly Action<object> action;

        public Command(Action<object> action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}