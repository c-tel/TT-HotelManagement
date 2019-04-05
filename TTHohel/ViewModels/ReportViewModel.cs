using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTHohel.Models;
using TTHotel.Contracts.Bookings;

namespace TTHohel.ViewModels
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        public ReportModel Model { get; private set; }

        public ReportViewModel()
        {
            Model = new ReportModel();
            SelectedDate = DateTime.Now;
        }


        private void ReinitItems()
        {
            ReportItems = new ObservableCollection<ReportItem>(Model.GetReportItems(SelectedDate));
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

        private ObservableCollection<ReportItem> _reportItems;
        public ObservableCollection<ReportItem> ReportItems
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
