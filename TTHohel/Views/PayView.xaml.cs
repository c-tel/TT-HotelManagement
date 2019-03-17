using System.Windows.Controls;
using TTHohel.ViewModels;

namespace TTHohel.Views
{
    /// <summary>
    /// Логика взаимодействия для PayView.xaml
    /// </summary>
    public partial class PayView : UserControl
    {
        private PayViewModel _payVM;

        public PayView()
        {
            InitializeComponent();
            _payVM = new PayViewModel();
            DataContext = _payVM;
        }
    }
}
