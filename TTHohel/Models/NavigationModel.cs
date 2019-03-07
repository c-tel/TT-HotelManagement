using System;
using TTHohel.Views;
using TTHohel.Windows;

namespace TTHohel.Models
{
    public enum ModesEnum
    {
        Login,
        Main,
        Settings
    }

    public class NavigationModel
    {

        private ContentWindow _contentWindow;
        private readonly LoginView _loginView;
        private readonly MainView _mainView;
        private readonly SettingsView _settingsView;

        public NavigationModel(ContentWindow contentWindow)
        {
            _contentWindow = contentWindow;
            _loginView = new LoginView();
            _mainView = new MainView();
            _settingsView = new SettingsView();
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

    }
}
