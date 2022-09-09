using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _16._2_桌面应用开发思维转变.Common
{
    internal class CommandHelper:ICommand
    {
        public Action<object> DoExecute { get; set; }

        public event EventHandler? CanExecuteChanged;

        public CommandHelper(Action<object> doExecute)
        {
            this.DoExecute = doExecute;
        }

        public bool CanExecute(object? parameter)
        {
            // 指令过滤
            return true;
        }

        public void Execute(object? parameter)
        {
            DoExecute?.Invoke(parameter);
        }
    }
}
