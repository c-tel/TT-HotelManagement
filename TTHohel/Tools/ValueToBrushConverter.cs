using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TTHohel.Contracts.Bookings;

namespace TTHohel.Tools
{
    class ValueToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            RoomInfo _ = value as RoomInfo;
            if (_ != null && parameter != null)
            {
                var statuses = _.DailyInfo;
                var index = parameter as int?;
                RoomDailyStatus currStatus = RoomDailyStatus.Free;
                if (index != null)
                    currStatus =  statuses.ElementAt(index.Value).Status;

                if (statuses.ElementAt(index.Value).Debt != 0)
                    return "#D81B0E";
                else
                {
                    switch (currStatus)
                    {
                        case RoomDailyStatus.Settled: return Brushes.LightSeaGreen;
                        case RoomDailyStatus.Booked: return Brushes.YellowGreen;
                        //case RoomDailyStatus.Free: return Brushes.WhiteSmoke;
                        default: return Brushes.WhiteSmoke;
                    }
                }
            }            
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
