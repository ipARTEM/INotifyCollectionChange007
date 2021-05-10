using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace INotifyCollectionChange007
{
    public class Command : ICommand
    {
        Action<object> Do;
        Func<object, bool> CanDo;

        public Command(Action<object> Do, Func<object, bool>CanDo)
        {
            this.CanDo = CanDo;
            this.Do = Do;
            CommandManager.RequerySuggested += (o, e) => Changed();
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => CanDo(parameter);

        public void Execute(object parameter) => Do(parameter);

        public void Changed() => CanExecuteChanged?.Invoke(null, EventArgs.Empty);

        
    }
}
