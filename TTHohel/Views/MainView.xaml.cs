using System.Windows.Controls;
using TTHohel.ViewModels;

namespace TTHohel.Views
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private MainViewModel _mainVM;

        public MainView()
        {
            InitializeComponent();
            _mainVM = new MainViewModel();
            DataContext = _mainVM;
        }

        private void MyDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
