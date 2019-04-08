using System.ComponentModel;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;

namespace TTHohel.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private ICommand _backCommand;
        private ICommand _addPersonnelCommand;
        private ICommand _addRoomCommand;

        public SettingsModel Model { get; private set; }

        public SettingsViewModel()
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
                InvokePropertyChanged(nameof(BackCommand));
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

        public ICommand AddPersonnelCommand
        {
            get
            {
                if (_addPersonnelCommand == null)
                {
                    _addPersonnelCommand = new RelayCommand<object>(AddPersonnelExecute, AddPersonnelCanExecute);
                }
                return _addPersonnelCommand;
            }
            set
            {
                _addPersonnelCommand = value;
                InvokePropertyChanged(nameof(AddPersonnelCommand));
            }
        }

        private bool AddPersonnelCanExecute(object obj)
        {
            return true;
        }

        private void AddPersonnelExecute(object obj)
        {
            Model.AddPersonnel();
        }

        public ICommand AddRoomCommand
        {
            get
            {
                if (_addRoomCommand == null)
                {
                    _addRoomCommand = new RelayCommand<object>(AddRoomExecute, AddRoomCanExecute);
                }
                return _addRoomCommand;
            }
            set
            {
                _addRoomCommand = value;
                InvokePropertyChanged(nameof(AddRoomCommand));
            }
        }

        private bool AddRoomCanExecute(object obj)
        {
            return true;
        }

        private void AddRoomExecute(object obj)
        {
            Model.AddRoom();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
    }
}
