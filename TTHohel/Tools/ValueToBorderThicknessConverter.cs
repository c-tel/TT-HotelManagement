using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TTHohel.Contracts.Bookings;

namespace TTHohel.Tools
{
    public class ValueToBorderThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            RoomInfo _ = value as RoomInfo;
            if (_ != null && parameter != null)
            {
                var statuses = _.DailyInfo;
                var index = parameter as string;

                var currInfo = statuses.FirstOrDefault(x => x.BookDate.ToString("dd-MM-yyyy") == index);

                if (currInfo.BookID == null)
                    return new Thickness(0);

                var thickness = new Thickness(0, 0.7, 0, 0.7);

                if (currInfo.IsStartDate)
                    thickness.Left = 0.7;
                if (currInfo.IsEndDate)
                    thickness.Right = 0.7;
                    //switch ()
                    //{
                    //    //case RoomDailyStatus.Settled: return Brushes.MediumSeaGreen;
                    //    //case RoomDailyStatus.Booked: return Brushes.YellowGreen;
                    //    default: return new Thickness(2);
                    //}
                return thickness;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
