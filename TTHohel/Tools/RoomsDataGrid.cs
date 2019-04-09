using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace TTHohel.Tools
{
    class RoomsDataGrid : DataGrid
    {

        public ObservableCollection<string> ColumnHeaders
        {
            get { return GetValue(ColumnHeadersProperty) as ObservableCollection<string>; }
            set
            {
                SetValue(ColumnHeadersProperty, value);
            }
        }

        public static readonly DependencyProperty ColumnHeadersProperty = DependencyProperty.Register("ColumnHeaders", typeof(ObservableCollection<string>), typeof(RoomsDataGrid), new PropertyMetadata(new PropertyChangedCallback(OnColumnsChanged)));

        static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = d as RoomsDataGrid;
            dataGrid.Columns.Clear();
            //Add Room Columns
            dataGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "Номер",
                Binding = new Binding("RoomNumber"),
                CellStyle = new Style
                {
                    Setters =
                    {
                        new Setter
                        {
                            Property = BorderThicknessProperty,
                            Value = new Thickness(0.5, 0.1, 0.5, 0.1)
                        },
                        new Setter
                        {
                            Property = BorderBrushProperty,
                            Value = Brushes.Black
                        }
                    }
                }
            });
            dataGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "Поверх",
                Binding = new Binding("Floor"),
                CellStyle = new Style
                {
                    Setters =
                    {
                        new Setter
                        {
                            Property = BorderThicknessProperty,
                            Value = new Thickness(0.5, 0.1, 0.5, 0.1)
                        },
                        new Setter
                        {
                            Property = BorderBrushProperty,
                            Value = Brushes.Black
                        }
                    }
                }
            });
            //Add States Columns
            foreach (var value in dataGrid.ColumnHeaders)
            {
                var column = new DataGridTextColumn()
                {
                    Header = value,
                    CanUserReorder = false,
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
                            },
                            new Setter
                            {
                                Property = BorderBrushProperty,
                                Value = Brushes.Black
            },
                            new Setter
                            {
                                Property = BorderThicknessProperty,
                                Value = new Binding
                                {
                                    ConverterParameter = value,
                                    Converter = new ValueToBorderThicknessConverter()
                                }
                                //Value = new Thickness (0.6)
                            }
                        }
                    }
                };
                dataGrid.Columns.Add(column);
            }
        }
    }
}
