using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Viewer.Uwp.Commands
{
    class TaskCommand : ICommand
    {
        private readonly Func<Task> commandFn;
        private bool enabled = true;

        public TaskCommand(Func<Task> commandFn)
        {
            this.commandFn = commandFn;
        }

        public bool CanExecute(object parameter)
        {
            return enabled;
        }

        public async void Execute(object parameter)
        {
            enabled = false;
            CanExecuteChanged?.Invoke(this, null);
            await commandFn();
            enabled = true;
            CanExecuteChanged?.Invoke(this, null);
        }

        public event EventHandler CanExecuteChanged;
    }
}
