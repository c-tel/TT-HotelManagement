﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TTHohel.Contracts.Bookings;
using TTHohel.Manager;
using TTHohel.Services;

namespace TTHohel.Models
{
    class MainModel
    {
        public event Action<RightsEnum> UserChanged;
        public event Action BookingsChanged;

        public MainModel()
        {
            Storage.Instance.UserChanged += OnUserChanged;
            Storage.Instance.BookingsChanged += OnBookingsChanged;
        }

        private void OnUserChanged(User user)
        {
            UserChanged?.Invoke(user.Rights);
        }

        private void OnBookingsChanged()
        {
            BookingsChanged?.Invoke();
        }

        public void Exit()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Login);
        }

        public void GoToSett()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Settings);
        }

        public void GoToStatistic()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Statistic);
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

        public void AddBooking()
        {
            NavigationManager.Instance.Navigate(ModesEnum.AddBooking);
        }

        public void GoToReport()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Report);
        }

        public void GoToAllBookings()
        {
            NavigationManager.Instance.Navigate(ModesEnum.AllBookings);
        }

        public void ProcessRoomSelection(int roomNumber)
        {
            var room = HotelApiClient.GetInstance().GetRoom(roomNumber);

            var data = new RoomDisplayData
            {
                CameFrom = ModesEnum.Main,
                Room = room,
                Mode = DisplayModes.Editing
            };

            Storage.Instance.ChangeRoomDisplayData(data);

            NavigationManager.Instance.Navigate(ModesEnum.Room);
        }
    }
}
