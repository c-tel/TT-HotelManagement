using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TTHohel.Views;

namespace TTHohel.ViewModels
{
    class StatisticViewModel
    {
        public ObservableCollection<TabItem> Tabs { get; set; }

        public StatisticViewModel()
        {
            Tabs = new ObservableCollection<TabItem>();
            Tabs.Add(new TabItem { Header = "Клієнти", Content = new ClientsListView() });
            Tabs.Add(new TabItem { Header = "Номери", Content = null });
            Tabs.Add(new TabItem { Header = "Працівники", Content = null });
        }



        public sealed class TabItem
        {
            public string Header { get; set; }
            public UserControl Content { get; set; }
        }

    }
}
