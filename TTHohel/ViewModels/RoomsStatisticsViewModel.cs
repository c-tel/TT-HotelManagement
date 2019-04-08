using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTHohel.Models;
using TTHotel.Contracts.Rooms;

namespace TTHohel.ViewModels
{
    public class RoomsStatisticsViewModel: INotifyPropertyChanged
    {
        public RoomsStatisticsViewModel()
        {
            DateTo = DateTime.Now;
            DateFrom = DateTime.Now.AddDays(-7);
            Model = new RoomStatisticsModel();
            RefreshStats();
        }

        private List<RoomStatisticsDTO> _statistics;
        private DateTime _dateTo;
        private DateTime _dateFrom;
        private readonly RoomStatisticsModel Model;

        public List<RoomStatisticsDTO> Statistics
        {
            get => _statistics;
            set
            {
                _statistics = value;
                InvokePropertyChanged(nameof(Statistics));
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
                    RefreshStats();
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
                    RefreshStats();
                    InvokePropertyChanged(nameof(DateTo));
                }
            }
        }


        private void RefreshStats()
        {
            Statistics = Model?.GetStatistics(DateFrom, DateTo);
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
