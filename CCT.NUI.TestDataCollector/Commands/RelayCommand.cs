using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CCT.NUI.TestDataCollector.Commands
{
    public class RelayCommand : ICommand
    {
        readonly Action<object> execute;
        readonly Predicate<object> canExecute;

        public RelayCommand(Action execute)
            : this((o) => execute(), (o) => true)
        { }

        public RelayCommand(Action<object> execute)
            : this(execute, (o) => true)
        { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }

    public class RelayCommand<T> : RelayCommand
    {
        public RelayCommand(Action<T> execute)
            : base((o) => execute((T) o))
        { }
    }
}
