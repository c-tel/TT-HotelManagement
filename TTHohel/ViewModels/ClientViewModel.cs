using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;
using TTHotel.Contracts.Clients;

namespace TTHohel.ViewModels
{
    class ClientViewModel : INotifyPropertyChanged
    {
        #region Private Fields
        private ClientDTO _client;
        private string _oldTel;

        private bool _isCreation;
        private bool _isEditing;
        private bool _isAdministrator;

        private ClientModel Model { get; }

        private ICommand _backCommand;
        private ICommand _addClient;
        private ICommand _saveClient;
        #endregion

        public ClientViewModel()
        {
            Model = new ClientModel();
            Client = new ClientDTO();

            Model.UserChanged += OnUserChanged;
            Model.ClientDisplayChanged += OnModeChanged;
        }

        #region Properties
        public ClientDTO Client
        {
            get { return _client; }
            set
            {
                if (_client != value)
                {
                    _client = value;
                    InvokePropertyChanged(nameof(Client));
                }
            }
        }
        #endregion

        #region Modes Properties
        public bool IsCreation
        {
            get { return _isCreation; }
            set
            {
                _isCreation = value;
                InvokePropertyChanged(nameof(IsCreation));
            }
        }

        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                InvokePropertyChanged(nameof(IsEditing));
            }
        }

        public bool IsAdministrator
        {
            get { return _isAdministrator; }
            set
            {
                _isAdministrator = value;
                InvokePropertyChanged(nameof(IsAdministrator));
            }
        }
        #endregion

        #region Commands
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                    _backCommand = new RelayCommand<object>(BackExecute, BackCanExecute);
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
            Model.GoBack();
        }

        public ICommand AddClient
        {
            get
            {
                if (_addClient == null)
                    _addClient = new RelayCommand<object>(AddClientExecute, AddClientCanExecute);
                return _addClient;
            }
            set
            {
                _addClient = value;
                InvokePropertyChanged(nameof(AddClient));
            }
        }

        private bool AddClientCanExecute(object obj)
        {
            return !(string.IsNullOrEmpty(Client.Name) ||
                    string.IsNullOrEmpty(Client.Surname) ||
                    string.IsNullOrEmpty(Client.TelNum) ||
                    string.IsNullOrEmpty(Client.Patronym));
        }

        private void AddClientExecute(object obj)
        {
            var res = Model.CreateNewClient(Client);
            if (res == 1)
            {
                MessageBox.Show("Клієнта створено.");
                Model.GoBack();
            } else if(res == 2)
            {
                MessageBox.Show("Клієнт з таким номером телефону вже є!", "Помилка");
            }
            else MessageBox.Show("Не вдалося створити клієнта", "Помилка");
        }

        public ICommand SaveClient
        {
            get
            {
                if (_saveClient == null)
                    _saveClient = new RelayCommand<object>(SaveClientExecute, SaveClientCanExecute);
                return _saveClient;
            }
            set
            {
                _saveClient = value;
                InvokePropertyChanged(nameof(SaveClient));
            }
        }

        private bool SaveClientCanExecute(object obj)
        {
            return !(string.IsNullOrEmpty(Client.Name) ||
                    string.IsNullOrEmpty(Client.Surname) ||
                    string.IsNullOrEmpty(Client.TelNum) ||
                    string.IsNullOrEmpty(Client.Patronym));
        }

        private void SaveClientExecute(object obj)
        {
            var res = Model.SaveClient(Client, _oldTel);
            if (res == 1)
            {
                MessageBox.Show("Інформацію змінено.");
                Model.GoBack();
            }
            else if(res == 2)
            {
                MessageBox.Show("Клієнт з таким номером телефону вже є.", "Помилка");
                Model.GoBack();
            }

            else MessageBox.Show("Не вдалося зберегти клієнта.", "Помилка");
        }
        #endregion

        private void OnUserChanged(RightsEnum rights)
        {
            IsAdministrator = !rights.HasFlag(RightsEnum.ClientDiscount);
        }

        private void OnModeChanged(ClientDisplayData data)
        {
            Client = data.Client;
            _oldTel = Client.TelNum;

            IsCreation = data.Mode.HasFlag(ClientViewModes.Creation);
            IsEditing = data.Mode.HasFlag(ClientViewModes.Editing);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
        #endregion
    }
}
