using System.Windows.Controls;
using TTHohel.ViewModels;

namespace TTHohel.Views
{
    /// <summary>
    /// Логика взаимодействия для PersonnelView.xaml
    /// </summary>
    public partial class PersonnelView : UserControl
    {
        public PersonnelView()
        {
            InitializeComponent();

            DataContext = new PersonnelViewModel();
        }
    }
}
