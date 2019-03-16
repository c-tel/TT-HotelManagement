using System.ComponentModel;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;
using System;
using System.Windows;
using System.Collections.ObjectModel;
using TTHohel.Contracts.Bookings;

namespace TTHohel.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private Visibility _settingsVisibility;

        private ICommand _exitCommand;
        private ICommand _settCommand;
        private ICommand _refreshCommand;

        private ObservableCollection<string> _columnHeaders;
        private ObservableCollection<RoomInfo> _infoTable;

        public MainModel Model { get; private set; }

        public MainViewModel()
        {
            Model = new MainModel();
            SettingsVisibility = Visibility.Collapsed;

            DateFrom  = DateTime.Today.Date;
            DateTo = DateTime.Today.AddDays(10);

            InfoTable = Model.ChangeInfoTable(DateFrom, DateTo);
            ColumnHeaders = Model.ChangeCollumnHeaders(DateFrom, DateTo);
        }

        public Visibility SettingsVisibility
        {
            get { return _settingsVisibility; }
            set
            {
                if (_settingsVisibility != value)
                {
                    _settingsVisibility = value;
                    InvokePropertyChanged(nameof(SettingsVisibility));
                }
            }
        }

        public ObservableCollection<string> ColumnHeaders {
            get { return _columnHeaders; }
            set
            {
                _columnHeaders = value;
                InvokePropertyChanged(nameof(ColumnHeaders));
            } }

        public ObservableCollection<RoomInfo> InfoTable {
            get { return _infoTable; }
            set
            {
                _infoTable = value;
                InvokePropertyChanged(nameof(InfoTable));
            }
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
                }
            }
        }

        public DateTime DateTo
        {
            get { return _dateTo; }
            set
            {
                if (_dateTo != value)
                {
                    _dateTo = value;
                    InvokePropertyChanged(nameof(DateTo));
                }
            }
        }

        public ICommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new RelayCommand<object>(ExitExecute, ExitCanExecute);
                }
                return _exitCommand;
            }
            set
            {
                _exitCommand = value;
                InvokePropertyChanged(nameof(ExitCommand));
            }
        }

        public ICommand SettCommand
        {
            get
            {
                if (_settCommand == null)
                {
                    _settCommand = new RelayCommand<object>(SettExecute, SettCanExecute);
                }
                return _settCommand;
            }
            set
            {
                _settCommand = value;
                InvokePropertyChanged(nameof(SettCommand));
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand<object>(RefreshExecute, RefreshCanExecute);
                }
                return _refreshCommand;
            }
            set
            {
                _refreshCommand = value;
                InvokePropertyChanged(nameof(RefreshCommand));
            }
        }

        private bool RefreshCanExecute(object obj)
        {
            if (_dateFrom <= _dateTo)
                return true;
            return false;
        }

        private void RefreshExecute(object obj)
        {
            InfoTable = Model.ChangeInfoTable(DateFrom, DateTo);
            ColumnHeaders= Model.ChangeCollumnHeaders(DateFrom, DateTo);
        }

        private void ExitExecute(object obj)
        {
            Model.Exit();
        }

        private bool ExitCanExecute(object obj)
        {
            return true;
        }
        private void SettExecute(object obj)
        {
            Model.GoToSett();
        }

        private bool SettCanExecute(object obj)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
    }
}
