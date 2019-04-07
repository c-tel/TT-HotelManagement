﻿using System.ComponentModel;
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

        private ICommand _backCommand;
        private ICommand _payCommand;
        private ICommand _openClientCommand;
        #endregion

        public BookingViewModel()
        {
            Model = new BookingModel();
            Model.BookingChanged += OnBookingChanged;
        }

        public BookingDTO BookingDTO
        {
            get { return _bookingDTO; }
            set
            {
                if (_bookingDTO != value)
                {
                    _bookingDTO = value;
                    InvokePropertyChanged(nameof(BookingDTO));

                    ToPay = Model.CalculateToPay(BookingDTO);
                }
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
