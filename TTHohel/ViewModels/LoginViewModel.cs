using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;

namespace TTHohel.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private string _name;
        private Visibility _errorVisibility;

        private ICommand _loginCommand;

        public LoginModel Model { get; private set; }

        public LoginViewModel()
        {
            Model = new LoginModel();
            ErrorVisibility = Visibility.Collapsed;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    InvokePropertyChanged(nameof(Name));
                }
            }
        }

        public Visibility ErrorVisibility
        {
            get { return _errorVisibility; }
            set
            {
                if (_errorVisibility != value)
                {
                    _errorVisibility = value;
                    InvokePropertyChanged(nameof(ErrorVisibility));
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
                InvokePropertyChanged(nameof(LoginCommand));
            }
        }

        private void LoginExecute(object obj)
        {
            var btn = obj as PasswordBox;

            if(!Model.Login(Name, btn.Password))
                ErrorVisibility = Visibility.Visible;
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
