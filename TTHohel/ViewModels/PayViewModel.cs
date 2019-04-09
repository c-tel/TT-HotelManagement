using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;
using TTHotel.Contracts.Payments;

namespace TTHohel.ViewModels
{
    class PayViewModel : INotifyPropertyChanged
    {
        #region Private Fields
        private PayModel Model { get; }

        private string _amount;
        private PaymentTypes _selectedPayment;

        private ICommand _backCommand;
        private ICommand _payCommand;
        #endregion

        public PayViewModel()
        {
            Model = new PayModel();
        }

        #region Properties
        public string Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                InvokePropertyChanged(nameof(Amount));
            }
        }

        public PaymentTypes SelectedPayment
        {
            get { return _selectedPayment; }
            set
            {
                _selectedPayment = value;
                InvokePropertyChanged(nameof(SelectedPayment));
            }
        }

        public List<DescriptionValueBinder> PaymentsList
        {
            get
            {
                return Enum.GetValues(typeof(PaymentTypes)).Cast<Enum>().Select(value => new
                DescriptionValueBinder
                {
                    Description = (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute))
                    as DescriptionAttribute).Description,
                    Value = (PaymentTypes)value
                }).ToList();
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
            Model.GoToBooking();
            Reset();
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

        private bool PayCanExecute(object obj) => true;

        private void PayExecute(object obj)
        {
            var res = Model.AddPayment(SelectedPayment, Amount);
            if (res == PayResult.Success)
            {
                Reset();
                Model.GoToBooking();
            }
            else
                MessageBox.Show(res.GetDescription());
        }
        #endregion

        private void Reset()
        {
            Amount = null;
            SelectedPayment = default(PaymentTypes);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
    }
}
