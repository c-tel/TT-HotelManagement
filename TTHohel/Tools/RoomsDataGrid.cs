using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TTHohel.Views;

namespace TTHohel.Tools
{
    class RoomsDataGrid : DataGrid
    {
        public ObservableCollection<int> ColumnHeaders
        {
            get { return GetValue(ColumnHeadersProperty) as ObservableCollection<int>; }
            set { SetValue(ColumnHeadersProperty, value); }
        }

        public static readonly DependencyProperty ColumnHeadersProperty = DependencyProperty.Register("ColumnHeaders", typeof(ObservableCollection<int>), typeof(RoomsDataGrid), new PropertyMetadata(new PropertyChangedCallback(OnColumnsChanged)));

        static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = d as RoomsDataGrid;
            dataGrid.Columns.Clear();
            //Add Person Column
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Номер", Binding = new Binding("RoomNumber") });
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Поверх", Binding = new Binding("Floor") });
            //Add Manufactures Columns
            foreach (var value in dataGrid.ColumnHeaders)
            {
                var column = new DataGridTextColumn()
                {
                    Header = value,
                    Width = 50,
                    Binding = new Binding("DailyInfo")
                    { ConverterParameter = value, Converter = new RoomsConverter() },
                    CellStyle = new Style
                    {
                        Setters = {
                            new Setter
                            {
                                Property = BackgroundProperty,
                                Value = new Binding
                                {
                                    ConverterParameter = value,
                                    Converter = new ValueToBrushConverter()
                                }
                            }
                        }
                    }
                };
                dataGrid.Columns.Add(column);
            }
        }
    }
}
