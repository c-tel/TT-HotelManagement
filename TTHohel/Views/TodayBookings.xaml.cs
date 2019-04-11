using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TTHohel.ViewModels;

namespace TTHohel.Views
{
    /// <summary>
    /// Логика взаимодействия для TodayBookings.xaml
    /// </summary>
    public partial class TodayBookings : UserControl
    {
        public TodayBookings()
        {
            InitializeComponent();
            DataContext = new TodayBookingsViewModel();
        }
    }
}
