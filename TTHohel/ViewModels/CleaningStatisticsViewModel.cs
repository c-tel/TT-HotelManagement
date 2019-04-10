using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTHohel.Services;
using TTHotel.Contracts.Cleanings;

namespace TTHohel.ViewModels
{
    public class CleaningStatisticsViewModel: INotifyPropertyChanged
    {

        public CleaningStatisticsViewModel()
        {
            SelectedDate = DateTime.Now;
        }


        private void ReinitItems()
        {
            Stats = new ObservableCollection<CleaningStatsDTO>(HotelApiClient.GetInstance().GetCleaningsStats(SelectedDate));
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

        private ObservableCollection<CleaningStatsDTO> _stats;
        public ObservableCollection<CleaningStatsDTO> Stats
        {
            get => _stats;
            set
            {
                _stats = value;
                InvokePropertyChanged(nameof(Stats));
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
