using System.Windows.Controls;
using TTHohel.ViewModels;

namespace TTHohel.Views
{
    /// <summary>
    /// Логика взаимодействия для AllBookingsView.xaml
    /// </summary>
    public partial class AllBookingsView : UserControl
    {
        public AllBookingsView()
        {
            InitializeComponent();

            DataContext = new AllBookingsViewModel();
        }
    }
}
