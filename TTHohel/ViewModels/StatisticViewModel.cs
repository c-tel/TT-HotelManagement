using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;
using TTHohel.Views;

namespace TTHohel.ViewModels
{
    class StatisticViewModel : INotifyPropertyChanged
    {
        private StatisticModel Model { get; }

        private ICommand _backCommand;

        public ObservableCollection<TabItem> Tabs { get; set; }

        public StatisticViewModel()
        {
            Model = new StatisticModel();

            Tabs = new ObservableCollection<TabItem>();
            Tabs.Add(new TabItem { Header = "Клієнти", Content = new ClientsListView() });
            Tabs.Add(new TabItem { Header = "Номери", Content = new RoomStatisticsView() });
            Tabs.Add(new TabItem { Header = "Працівники", Content = new CleaningStatsView() });
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

        public sealed class TabItem
        {
            public string Header { get; set; }
            public UserControl Content { get; set; }
        }

    }
