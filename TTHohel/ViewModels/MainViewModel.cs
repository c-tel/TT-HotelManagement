using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Windows;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Linq;

namespace TTHohel.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private List<DateTime> _datesList;

        private ICommand _exitCommand;
        private ICommand _settCommand;
        private ICommand _refreshCommand;

        public ObservableCollection<RoomInfo> InfoTable { get; set; }
        public ObservableCollection<int> ColumnHeaders { get; set; }

        public MainModel Model { get; private set; }

        public MainViewModel()
        {
            Model = new MainModel();
            _dateFrom  = DateTime.Today.Date;
            _dateTo = DateTime.Today.AddDays(10);
            _datesList = DatesList();

            ColumnHeaders = new ObservableCollection<int>();


            // string output = File.ReadAllText("room_data.json");
            // var inp = JsonConvert.DeserializeObject<List<RoomInfo>>(output);
            var inp = new List<RoomInfo>();

            foreach (var i in Enumerable.Range(0, 10))
            {
                ColumnHeaders.Add(i);
            }

            InfoTable = new ObservableCollection<RoomInfo>(inp);
        }

        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set
            {
                if (_dateFrom != value)
                {
                    _dateFrom = value;
                    InvokePropertyChanged("DateFrom");
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
                    InvokePropertyChanged("DateTo");
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
                InvokePropertyChanged("ExitCommand");
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
                InvokePropertyChanged("SettCommand");
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
                InvokePropertyChanged("RefreshCommand");
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
            MessageBox.Show(DateTo.Date.ToString());
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

        private List<DateTime> DatesList()
        {
            List<DateTime> dateTimes = new List<DateTime>();
            DateTime curr = _dateFrom;
            while (curr != _dateTo.AddDays(1))
            {
                dateTimes.Add(curr);
                curr = curr.AddDays(1);
            }
            return dateTimes;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
    }
}
