using System.Windows;
using TTHohel.Manager;
using TTHohel.Models;
using TTHohel.Services;
using TTHohel.Windows;
using TTHotel.Contracts.Auth;

namespace TTHohel
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly Credentials AUTHORIZATION_STUB = new Credentials { Login = "admin1", Password = "admin1" };
        
        // uncomment for prod
        // public static readonly Credentials AUTHORIZATION_STUB = null;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ContentWindow contentWindow = new ContentWindow();
            NavigationModel navigationModel = new NavigationModel(contentWindow);
            NavigationManager.Instance.Initialize(navigationModel);
            contentWindow.Show();
            if (AUTHORIZATION_STUB != null)
            {
                var authResp = HotelApiClient.GetInstance().Login(AUTHORIZATION_STUB.Login, AUTHORIZATION_STUB.Password);
                if (authResp != null)
                {
                    var userRole = authResp.User.Role;
                    Storage.Instance.ChangeUser(new User(LoginModel.RightsDictionary[userRole]));

                    NavigationManager.Instance.Navigate(ModesEnum.Main);
                }
            }
            else
            {
                navigationModel.Navigate(ModesEnum.Login);
            }
        }
    }
}
