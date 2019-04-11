using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;
using TTHohel.Views;

namespace TTHohel.ViewModels
{
    class AllBookingsViewModel : INotifyPropertyChanged
    {
        private AllBookingsModel Model { get; }

        private ICommand _backCommand;

        public ObservableCollection<TabItem> Tabs { get; set; }

        public AllBookingsViewModel()
        {
            Model = new AllBookingsModel();

            Tabs = new ObservableCollection<TabItem>();
            Tabs.Add(new TabItem { Header = "Заплановані заїзди", Content = new ClientsListView() });
            Tabs.Add(new TabItem { Header = "Заборгованості", Content = new RoomStatisticsView() });
        }

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
