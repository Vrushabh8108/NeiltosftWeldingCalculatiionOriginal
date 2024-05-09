using System.Windows.Input;

namespace CommandDesignPattern
{
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action<object> _Execute;

        private Predicate<object> _CanExecute{ get; set; }


        public RelayCommand(Action<object> executeMethod,Predicate<object> canExecuteMethod)
        {
            _CanExecute = canExecuteMethod; 
            _Execute = executeMethod;
        }
        public bool CanExecute(object? parameter)
        {
           return _CanExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _Execute(parameter);
        }
    }
}