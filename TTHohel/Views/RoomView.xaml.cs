using System.Windows.Controls;
using TTHohel.ViewModels;

namespace TTHohel.Views
{
    /// <summary>
    /// Логика взаимодействия для RoomView.xaml
    /// </summary>
    public partial class RoomView : UserControl
    {
        public RoomView()
        {
            InitializeComponent();
            DataContext = new RoomViewModel();
        }
    }
}
