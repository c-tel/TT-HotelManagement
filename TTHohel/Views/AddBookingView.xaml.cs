using System.Windows.Controls;
using TTHohel.ViewModels;

namespace TTHohel.Views
{
    /// <summary>
    /// Логика взаимодействия для AddBookingView.xaml
    /// </summary>
    public partial class AddBookingView : UserControl
    {
        private AddBookingViewModel _addVM;

        public AddBookingView()
        {
            InitializeComponent();

            _addVM = new AddBookingViewModel();
            DataContext = _addVM;
        }
    }
}
