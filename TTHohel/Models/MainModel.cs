using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using TTHohel.Contracts.Bookings;
using TTHohel.Manager;
using TTHohel.Services;

namespace TTHohel.Models
{
    class MainModel
    {
        public event Action<RightsEnum> UserChanged;

        public MainModel()
        {
            Storage.Instance.UserChanged += OnUserChanged;
        }

        private void OnUserChanged(User user)
        {
            UserChanged?.Invoke(user.Rights);
        }

        public void Exit()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Login);
        }

        public void GoToSett()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Settings);
        }

        public ObservableCollection<RoomInfo> ChangeInfoTable(DateTime dateFrom, DateTime dateTo)
        {
            var inp = HotelApiClient.GetInstance().RoomInfos(dateFrom, dateTo);
            return new ObservableCollection<RoomInfo>(inp);
        }

        public ObservableCollection<string> ChangeCollumnHeaders(DateTime dateFrom, DateTime dateTo)
        {
            var datesList = DatesRange(dateFrom, dateTo);
            ObservableCollection<string> vs = new ObservableCollection<string>();
            foreach (DateTime i in datesList)
                vs.Add(i.ToString("dd-MM-yyyy"));
            return vs;
        }

        public List<DateTime> DatesRange(DateTime dateFrom, DateTime dateTo)
        {
            var datesList = new List<DateTime>();
            var i = dateFrom;
            while (i <= dateTo)
            {
                datesList.Add(i);
                i = i.AddDays(1);
            }
            return datesList;
        }

        public void ProcessBookingSelection(int bookingId)
        {
            var res = HotelApiClient.GetInstance().GetBookingById(bookingId);
            Storage.Instance.ChangeBooking(res);

            NavigationManager.Instance.Navigate(ModesEnum.Booking);
        }

        internal void AddBooking()
        {
            NavigationManager.Instance.Navigate(ModesEnum.AddBooking);
        }
    }
}
