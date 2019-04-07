using System.Windows.Controls;
using TTHohel.ViewModels;

namespace TTHohel.Views
{
    /// <summary>
    /// Логика взаимодействия для AddClientView.xaml
    /// </summary>
    public partial class ClientView : UserControl
    {
        private ClientViewModel _addClVM;

        public ClientView()
        {
            InitializeComponent();

            _addClVM = new ClientViewModel();
            DataContext = _addClVM;
        }
    }
}
