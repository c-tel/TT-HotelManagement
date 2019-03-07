using System.ComponentModel;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;

namespace TTHohel.ViewModels
{
    class SettingViewModel : INotifyPropertyChanged
    {
        private ICommand _backCommand;

        public SettingsModel Model { get; private set; }

        public SettingViewModel()
        {
            Model = new SettingsModel();
        }

        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new RelayCommand<object>(BackExecute, BackCanExecute);
                }
                return _backCommand;
            }
            set
            {
                _backCommand = value;
                InvokePropertyChanged("BackCommand");
            }
        }

        private bool BackCanExecute(object obj)
        {
            return true;
        }

        private void BackExecute(object obj)
        {
            Model.GoToMain();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
    }
}
