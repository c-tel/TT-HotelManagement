using System.ComponentModel;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;

namespace TTHohel.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private string _name;
        //private string _password;

        private ICommand _loginCommand;
        //private ICommand _exitCommand;

        public LoginModel Model { get; private set; }

        public LoginViewModel()
        {
            Model = new LoginModel();
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    InvokePropertyChanged("Name");
                }
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new RelayCommand<object>(LoginExecute, LoginCanExecute);
                }

                return _loginCommand;
            }
            set
            {
                _loginCommand = value;
                InvokePropertyChanged("LoginCommand");
            }
        }

        private void LoginExecute(object obj)
        {
            Model.Login(Name);
        }

        private bool LoginCanExecute(object obj)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
    }
}
