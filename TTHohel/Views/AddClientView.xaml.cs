using System.Windows.Controls;
using TTHohel.ViewModels;

namespace TTHohel.Views
{
    /// <summary>
    /// Логика взаимодействия для AddClientView.xaml
    /// </summary>
    public partial class AddClientView : UserControl
    {
        private AddClientViewModel _addClVM;

        public AddClientView()
        {
            InitializeComponent();

            _addClVM = new AddClientViewModel();
            DataContext = _addClVM;
        }
    }
}
