using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using System.Collections.Generic;


namespace ChatClient.MVVM.Core
{
    public class RelayCommand : ICommand


    {
        private Action<object> execute; //holds the action to be executed when the command is invoked 
        private Func<object, bool> canExecute; //holds a function that determines if the command can be executed 

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value;  }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) //annonymoys
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }



        public bool CanExecute(object? parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            this.execute(parameter);
        }
    }
}

