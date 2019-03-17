using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TTHohel.UserControls.DropdownAutocomplete
{
    /// <summary>
    /// Логика взаимодействия для DropdownAutocomplete.xaml
    /// </summary>
    public partial class DropdownWithSearch : UserControl, INotifyPropertyChanged
    {

        #region ItemsProp
        public object Items
        {
            get { return (ObservableCollection)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<DropdownItem>),
                typeof(DropdownWithSearch), 
                new PropertyMetadata(new PropertyChangedCallback(OnSearchParamsChanged)));

        static void OnSearchParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dropdown = d as DropdownWithSearch;
            dropdown.FilteredItems = new ObservableCollection<DropdownItem>(dropdown.Items
                    .Where(i => i.ItemTitle.Contains(dropdown.SearchText)));
         
        }
        #endregion
        public I
        private ObservableCollection<DropdownItem> _filteredItems;

        private ObservableCollection<DropdownItem> FilteredItems
        {
            get { return _filteredItems; }
            set
            {
                _filteredItems = value;
                InvokePropertyChanged(nameof(FilteredItems));
            }
        }

        private string _searchText;

        public event PropertyChangedEventHandler PropertyChanged;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnSearchParamsChanged(this, new DependencyPropertyChangedEventArgs());
            }
        }


        public DropdownWithSearch()
        {
            InitializeComponent();
        }

        private void InvokePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
    }
}
