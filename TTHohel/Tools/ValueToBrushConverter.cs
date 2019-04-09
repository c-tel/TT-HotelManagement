using System;
using System.Linq;
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
                var index = parameter as string;

                var currInfo = statuses.FirstOrDefault(x => x.BookDate.ToString("dd-MM-yyyy") == index);

                RoomDailyStatus currStatus =  currInfo.Status;

                if (currInfo.Debt > 0 && currInfo.Debt != null)
                    return "#D81B0E";
                else
                {
                    switch (currStatus)
                    {
                        case RoomDailyStatus.Settled: return Brushes.MediumSeaGreen;
                        case RoomDailyStatus.Booked: return Brushes.YellowGreen;
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
