using System;
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
        Pay,
        AddBooking,
        AllClients,
        Report,
        Client
    }

    public class NavigationModel
    {
        private ContentWindow _contentWindow;
        private readonly LoginView _loginView;
        private readonly MainView _mainView;
        private readonly SettingsView _settingsView;
        private readonly BookingView _bookingView;
        private readonly PayView _payView;
        private readonly ReportView _reportView;
        private readonly AddBookingView _addBookingView;
        private readonly ClientView _clientView;

        public NavigationModel(ContentWindow contentWindow)
        {
            _contentWindow = contentWindow;
            _loginView = new LoginView();
            _mainView = new MainView();
            _settingsView = new SettingsView();
            _bookingView = new BookingView();
            _payView = new PayView();
            _reportView = new ReportView();
            _addBookingView = new AddBookingView();
            _clientView = new ClientView();
        }

        public void Navigate(ModesEnum mode)
        {
            switch (mode)
            {
                case ModesEnum.Login:
                    _contentWindow.ContentControl.Content = _loginView;
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

    }
}
