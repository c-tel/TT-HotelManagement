﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;
using System;
using System.Windows;
using System.Collections.ObjectModel;
using TTHohel.Contracts.Bookings;
using TTHohel.Services;

namespace TTHohel.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private List<DateTime> _datesList;
        private Visibility _settingsVisibility;

        private ICommand _exitCommand;
        private ICommand _settCommand;
        private ICommand _refreshCommand;

        ObservableCollection<string> _columnHeaders;
        public ObservableCollection<RoomInfo> InfoTable { get; set; }

        public MainModel Model { get; private set; }

        public MainViewModel()
        {
            Model = new MainModel();
            SettingsVisibility = Visibility.Collapsed;

            _datesList = new List<DateTime>();
            DateFrom  = DateTime.Today.Date;
            DateTo = DateTime.Today.AddDays(10);

            ColumnHeaders = ChangeCollumnHeaders();

            // string output = File.ReadAllText("room_data.json");
            // var inp = JsonConvert.DeserializeObject<List<RoomInfo>>(output);
            var inp = HotelApiClient.GetInstance().RoomInfos(DateFrom, DateTo);

            InfoTable = new ObservableCollection<RoomInfo>(inp);
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

        private void ChangeDatesList()
        {
            _datesList.Clear();
            var i = _dateFrom;
            while (i <= _dateTo){
                _datesList.Add(i);
                i = i.AddDays(1);
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
            ColumnHeaders= ChangeCollumnHeaders();
        }

        private ObservableCollection<string> ChangeCollumnHeaders()
        {
            ChangeDatesList();
            ObservableCollection<string> vs = new ObservableCollection<string>();
            foreach (DateTime i in _datesList)
                vs.Add(i.ToString("dd-MM-yyyy"));
            return vs;
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
