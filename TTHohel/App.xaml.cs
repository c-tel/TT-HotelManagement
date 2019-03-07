using System.Windows;
using TTHohel.Manger;
using TTHohel.Models;
using TTHohel.Windows;

namespace TTHohel
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ContentWindow contentWindow = new ContentWindow();
            NavigationModel navigationModel = new NavigationModel(contentWindow);
            NavigationManager.Instance.Initialize(navigationModel);
            contentWindow.Show();
            navigationModel.Navigate(ModesEnum.Login);
        }
    }
}
