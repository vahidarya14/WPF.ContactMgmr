using System;
using System.Windows.Input;

namespace A.WPF
{
    public class Cmd:ICommand
    {
        public Func<object, bool> CanExec { get; set; }
        public Action<object> Exec { get; set; }

        public Cmd(Action<object> exec, Func<object, bool> canExec=null)
        {
            Exec = exec;
            CanExec = canExec;
        }

        public bool CanExecute(object parameter)
        {
            return CanExec==null || CanExec(parameter);
        }

        public void Execute(object parameter)
        {
            Exec(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
