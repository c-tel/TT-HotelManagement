using System.Windows;
using System.Windows.Controls;
using TTHohel.ViewModels;

namespace TTHohel.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        private LoginViewModel _loginVM;

        public LoginView()
        {
            InitializeComponent();
            _loginVM = new LoginViewModel();
            DataContext = _loginVM;
        }

        private void BtnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Ви впевнені, що хочете вийти?", "Увага!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}
