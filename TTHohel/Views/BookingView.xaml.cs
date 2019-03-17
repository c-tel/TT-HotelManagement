using System.Windows.Controls;
using TTHohel.ViewModels;

namespace TTHohel.Views
{
    /// <summary>
    /// Логика взаимодействия для BookingView.xaml
    /// </summary>
    public partial class BookingView : UserControl
    {
        private BookingViewModel _bookingVM;

        public BookingView()
        {
            InitializeComponent();
            _bookingVM = new BookingViewModel();
            DataContext = _bookingVM;
        }
    }
}
