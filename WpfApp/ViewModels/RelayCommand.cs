using System.Windows.Input;

namespace WpfApp.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action action;
        private readonly Func<bool>? canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action action, Func<bool>? canExecute = null)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute();
        }

        // Relier au click du button
        public void Execute(object? parameter)
        {
            action();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> action;
        private readonly Func<T,bool>? canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<T> action, Func<T, bool>? canExecute = null)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || (parameter is T t && canExecute(t));
        }

        // Relier au click du button
        public void Execute(object? parameter)
        {
            if(parameter is T t)
                action(t);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}