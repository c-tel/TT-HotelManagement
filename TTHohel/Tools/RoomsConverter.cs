using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using TTHohel.Contracts.Bookings;

namespace TTHohel.Tools
{
    public class RoomsConverter : IValueConverter
    {

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var statuses = value as ObservableCollection<RoomDailyInfo>;
            var index = parameter as int?;
            if (index != null)
                if (statuses.ElementAt(index.Value).Debt > 0)
                    return statuses.ElementAt(index.Value).Debt;
                return "";
                //return statuses.ElementAt(index.Value).Status.ToString();
            //return null;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
