using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;
using TTHotel.Contracts.Bookings;

namespace TTHohel.ViewModels
{
    class BookingViewModel : INotifyPropertyChanged
    {
        #region Private Fields
        private BookingDTO _bookingDTO;
        private double _toPay;
        private BookingModel Model { get; }

        private DateTime _dateFrom;
        private DateTime _endDateDisplay;

        private ICommand _backCommand;
        private ICommand _payCommand;
        private ICommand _editCommand;
        private ICommand _openClientCommand;
        private ICommand _settleCommand;
        private ICommand _closeCommand;
        private ICommand _cancelCommand;
        private double _generalPrice;
        #endregion

        public BookingViewModel()
        {
            Model = new BookingModel();
            Model.BookingChanged += OnBookingChanged;
        }

        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set
            {
                if (_dateFrom != value)
                {
                    _dateFrom = value;
                    InvokePropertyChanged(nameof(DateFrom));
                    UpdateEndDisplayDate();

                }
            }
        }

        public DateTime EndDateDisplay
        {
            get { return _endDateDisplay; }
            set
            {
                if (_endDateDisplay != value)
                {
                    _endDateDisplay = value;
                    InvokePropertyChanged(nameof(EndDateDisplay));
                }
            }
        }

        public BookingDTO BookingDTO
        {
            get { return _bookingDTO; }
            set
            {
                if (_bookingDTO != value)
                {
                    _bookingDTO = value;
                }
            }
        }

        public bool IsBookingActive { get => BookingDTO?.EndDateReal == new DateTime(); }
        public bool IsNotSettled{ get => BookingDTO?.Book_state != BookingStates.Settled; }

        public string BookingState
        {
            get
            {
                if (!IsBookingActive)
                    return "закрито";
                return BookingDTO?.Book_state.GetDescription();
            }
        }

        public double ToPay
        {
            get { return _toPay; }
            set
            {
                _toPay = value;
                InvokePropertyChanged(nameof(ToPay));
            }
        }

        public double GeneralPrice
        {
            get { return _generalPrice; }
            set
            {
                _generalPrice = value;
                InvokePropertyChanged(nameof(GeneralPrice));
            }
        }


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
            Model.GoToMain();
        }

        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new RelayCommand<object>(EditExecute, EditCanExecute);
                return _editCommand;
            }
            set
            {
                _editCommand = value;
                InvokePropertyChanged(nameof(EditCommand));
            }
        }

        private bool EditCanExecute(object obj)
        {
            return BookingDTO?.Book_state != BookingStates.Canceled;
        }

        private void EditExecute(object obj)
        {
            if (Model.Edit(BookingDTO, DateFrom))
                MessageBox.Show("Бронювання змінено.");
            else MessageBox.Show("Не вдалося оновити стан бронювання.", "Помилка");

        }

        public ICommand SettleCommand
        {
            get
            {
                if (_settleCommand == null)
                    _settleCommand = new RelayCommand<object>(SettleExecute, SettleCanExecute);
                return _settleCommand;
            }
            set
            {
                _settleCommand = value;
                InvokePropertyChanged(nameof(SettleCommand));
            }
        }

        private bool SettleCanExecute(object obj)
        {
            return BookingDTO?.Book_state != BookingStates.Settled &&
                BookingDTO?.StartDate <= DateTime.Today.Date;
        }

        private void SettleExecute(object obj)
        {
            if (string.IsNullOrWhiteSpace(Model.GetClient(BookingDTO.ClientTel).Passport))
            {
                MessageBox.Show("Щоб поселити клієнта, додайте паспортні дані.", "Помилка");
            }
            else
            {
                if (Model.Settle(BookingDTO))
                {

                }
                else MessageBox.Show("Не вдалося оновити стан бронювання.", "Помилка");
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand<object>(CloseExecute, CloseCanExecute);
                return _closeCommand;
            }
            set
            {
                _closeCommand = value;
                InvokePropertyChanged(nameof(CloseCommand));
            }
        }

        private bool CloseCanExecute(object obj)
        {
            return BookingDTO?.Book_state == BookingStates.Settled &&
                    IsBookingActive &&
                    ToPay == 0;
        }

        private void CloseExecute(object obj)
        {
            if (Model.Close(BookingDTO))
            {
                MessageBox.Show("Бронювання оновлено.");
            }
            else MessageBox.Show("Не вдалося оновити стан бронювання.", "Помилка");
        }

        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand<object>(CancelExecute, CancelCanExecute);
                return _cancelCommand;
            }
            set
            {
                _cancelCommand = value;
                InvokePropertyChanged(nameof(CancelCommand));
            }
        }

        private bool CancelCanExecute(object obj)
        {
            return BookingDTO?.Book_state != BookingStates.Settled &&
                    BookingDTO?.Book_state != BookingStates.Canceled;
        }

        private void CancelExecute(object obj)
        {
            if (Model.Cancel(BookingDTO))
            {

            }
            else MessageBox.Show("Не вдалося оновити стан бронювання.", "Помилка");
        }

        public ICommand PayCommand
        {
            get
            {
                if (_payCommand == null)
                    _payCommand = new RelayCommand<object>(PayExecute, PayCanExecute);
                return _payCommand;
            }
            set
            {
                _payCommand = value;
                InvokePropertyChanged(nameof(PayCommand));
            }
        }

        private bool PayCanExecute(object obj)
        {
            return ToPay > 0;
        }

        private void PayExecute(object obj)
        {
            Model.GoToPay();
        }

        public ICommand OpenClientCommand
        {
            get
            {
                if (_openClientCommand == null)
                    _openClientCommand = new RelayCommand<object>(OpenClientExecute, OpenClientCanExecute);
                return _openClientCommand;
            }
            set
            {
                _openClientCommand = value;
                InvokePropertyChanged(nameof(OpenClientCommand));
            }
        }

        private bool OpenClientCanExecute(object obj)
        {
            return true;
        }

        private void OpenClientExecute(object obj)
        {
            Model.GoToClient(BookingDTO.ClientTel);
        }
        #endregion

        private void OnBookingChanged(BookingDTO obj)
        {
            BookingDTO = obj;
            DateFrom = obj.StartDate;

            InvokePropertyChanged(nameof(BookingDTO));
            InvokePropertyChanged(nameof(BookingState));
            InvokePropertyChanged(nameof(IsBookingActive));
            InvokePropertyChanged(nameof(IsNotSettled));

            ToPay = BookingModel.CalculateToPay(BookingDTO);
            GeneralPrice = BookingModel.CalculateGeneral(BookingDTO);
        }

        private void UpdateEndDisplayDate()
        {
            EndDateDisplay = DateFrom.AddDays(1);

            if (BookingDTO.EndDate < EndDateDisplay)
                BookingDTO.EndDate = EndDateDisplay;
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
