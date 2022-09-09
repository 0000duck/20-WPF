using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AlbertWPF_Calculate.Command
{
    public class CommandHelper : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public Action<object> DoExecute { get; set; }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            DoExecute?.Invoke(parameter);
        }

        public CommandHelper(Action<object> doExecute)
        {
            DoExecute = doExecute;
        }
    }
}
