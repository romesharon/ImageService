using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageServiceGUI
{
    public class DelegateCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private ExcuteMethod<T> executeMethod;
        private CanExecuteMethod<T> canExecuteMethod;

        public delegate void ExcuteMethod<T>(object obj);
        public delegate bool CanExecuteMethod<T>(object obj);

        public DelegateCommand(ExcuteMethod<T> executeMethod, CanExecuteMethod<T> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecuteMethod(parameter);
        }

        public void Execute(object parameter)
        {
            this.executeMethod(parameter);
        }

        public void RaiseCanExecuteChange()
        {
            this.CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
