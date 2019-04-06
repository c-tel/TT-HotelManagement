using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;
using TTHotel.Contracts.Clients;

namespace TTHohel.ViewModels
{
    class AddClientViewModel : INotifyPropertyChanged
    {
        #region Private Fields
        private ClientDTO _client;

        private ClientModel Model { get; }

        private ICommand _backCommand;
        private ICommand _addClient;
        #endregion

        public AddClientViewModel()
        {
            Model = new ClientModel();
            Client = new ClientDTO();
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
            return true;
        }

        private void AddClientExecute(object obj)
        {
            if (Model.CreateNewClient(Client))
                Model.GoBack();
            else MessageBox.Show("Не вдалося створити клієнта","Помилка");
        }
        #endregion

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
