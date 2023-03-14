using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collaborate.Microsoft.MAUI
{
    using global::Microsoft.Graph.Models;
    using System.Windows.Input;

    public class ModelCommand<T> : ICommand 
    {
        private readonly Predicate<T?>? canExecute;

        private readonly Action<T?>? execute;

        private readonly T Param;

        public ModelCommand(Action<T?> execute, T param, Predicate<T?>? canExecute = null )
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.Param = param;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (parameter is T parameterT)
            {
                return canExecute == null || canExecute(parameterT);
            }

            return true;
        }

        public void Execute(object? parameter)
        {
           // dynamic param = execute.Target?.team.Id;
           // var id = ((Team)execute.Target).Id;
           // if (parameter is T parameterT)
            {
                execute?.Invoke(Param);
            }
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
