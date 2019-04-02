using System.Windows.Controls;
using TTHohel.ViewModels;

namespace TTHohel.Views
{
    /// <summary>
    /// Логика взаимодействия для AllClientsView.xaml
    /// </summary>
    public partial class AllClientsView : UserControl
    {
        private AllClientsViewModel _allClientsVM;

        public AllClientsView()
        {
            InitializeComponent();

            _allClientsVM = new AllClientsViewModel();
            DataContext = _allClientsVM;
        }
    }
}
