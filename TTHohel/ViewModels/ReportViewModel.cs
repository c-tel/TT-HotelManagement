using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;
using TTHotel.Contracts.Bookings;
using TTHotel.Contracts.Payments;

namespace TTHohel.ViewModels
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        private ICommand _printCommand;
        public ReportModel Model { get; private set; }
        
        public ReportViewModel()
        {
            Model = new ReportModel();
            SelectedDate = DateTime.Now;
        }


        private void ReinitItems()
        {
            ReportItems = new ObservableCollection<ReportItemModel>(Model.GetReportItems(SelectedDate).Select(i => new ReportItemModel
            {
                Amount = i.Amount,
                PaymentTime = i.PaymentDate.ToString("HH:mm"),
                PaymentType = i.PaymentType == PaymentTypes.Card ? "Карткою" : "Готівкою",
                RoomNum = i.RoomNum
        }));
        }

        public ICommand PrintCommand
        {
            get
            {
                if (_printCommand == null)
                {
                    _printCommand = new RelayCommand<object>(PrintExecute, PrintCanExecute);
                }
                return _printCommand;
            }
            set
            {
                _printCommand = value;
                InvokePropertyChanged(nameof(PrintCommand));
            }
        }

        private void PrintExecute(object obj)
        {
            Model.CreateDocument(ReportItems, SelectedDate);
        }

        private bool PrintCanExecute(object obj) => ReportItems.Any();

        private ICommand _backCommand;

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
            Model.Back();
        }
        #region ViewParams
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                ReinitItems();
                InvokePropertyChanged(nameof(SelectedDate));
            }
        }

        private ObservableCollection<ReportItemModel> _reportItems;
        public ObservableCollection<ReportItemModel> ReportItems
        {
            get => _reportItems;
            set
            {
                _reportItems = value;
                InvokePropertyChanged(nameof(ReportItems));
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
