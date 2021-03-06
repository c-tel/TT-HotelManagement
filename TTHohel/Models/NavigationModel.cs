﻿using System;
using TTHohel.Views;
using TTHohel.Windows;

namespace TTHohel.Models
{
    public enum ModesEnum
    {
        Login,
        Main,
        Settings,
        Booking,
        Statistic,
        Pay,
        AddBooking,
        AllClients,
        Report,
        Client,
        Personnel,
        Room,
        Maid,
        AllBookings
    }

    public class NavigationModel
    {
        private ContentWindow _contentWindow;
        private readonly MainView _mainView;
        private readonly SettingsView _settingsView;
        private readonly BookingView _bookingView;
        private readonly PayView _payView;
        private readonly ReportView _reportView;
        private readonly AddBookingView _addBookingView;
        private readonly ClientView _clientView;
        private readonly PersonnelView _personnelView;

        public NavigationModel(ContentWindow contentWindow)
        {
            _contentWindow = contentWindow;
            _mainView = new MainView();
            _settingsView = new SettingsView();
            _bookingView = new BookingView();
            _payView = new PayView();
            _reportView = new ReportView();
            _addBookingView = new AddBookingView();
            _clientView = new ClientView();
            _personnelView = new PersonnelView();
        }

        public void Navigate(ModesEnum mode)
        {
            switch (mode)
            {
                case ModesEnum.Login:
                    _contentWindow.ContentControl.Content = new LoginView();
                    break;
                case ModesEnum.Main:
                    _contentWindow.ContentControl.Content = _mainView;
                    break;
                case ModesEnum.Settings:
                    _contentWindow.ContentControl.Content = _settingsView;
                    break;
                case ModesEnum.Booking:
                    _contentWindow.ContentControl.Content = _bookingView;
                    break;
                case ModesEnum.Pay:
                    _contentWindow.ContentControl.Content = _payView;
                    break;
                case ModesEnum.AddBooking:
                    _contentWindow.ContentControl.Content = _addBookingView;
                    break;
                case ModesEnum.Report:
                    _contentWindow.ContentControl.Content = _reportView;
                    break;
                case ModesEnum.Client:
                    _contentWindow.ContentControl.Content = _clientView;
                    break;
                case ModesEnum.Statistic:
                    _contentWindow.ContentControl.Content = new StatisticView();
                    break;
                case ModesEnum.Personnel:
                    _contentWindow.ContentControl.Content = _personnelView;
                    break;
                case ModesEnum.Room:
                    _contentWindow.ContentControl.Content = new RoomView();
                    break;
                case ModesEnum.Maid:
                    _contentWindow.ContentControl.Content = new MaidMainView();
                    break;
                case ModesEnum.AllBookings:
                    _contentWindow.ContentControl.Content = new AllBookingsView();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

    }
}
