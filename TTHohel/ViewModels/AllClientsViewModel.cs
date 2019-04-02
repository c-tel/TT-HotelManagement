using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TTHohel.Models;
using TTHotel.Contracts.Clients;

namespace TTHohel.ViewModels
{
    class AllClientsViewModel : INotifyPropertyChanged
    {
        #region Private Fields
        private List<ClientDTO> _clientsList;

        private AllClientsModel Model { get; }
        #endregion

        public AllClientsViewModel()
        {
            Model = new AllClientsModel();

            _clientsList = Model.GetClientsList();
        }

        #region Properties
        public List<ClientDTO> ClientsList
        {
            get { return _clientsList; }
            set
            {
                _clientsList = value;
                InvokePropertyChanged(nameof(ClientsList));
            }
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
