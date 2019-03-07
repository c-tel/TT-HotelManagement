using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.ObjectModel;

namespace TTHohel.Models
{
    public enum RoomType
    {
        Standard, Econom, DeLuxe
    }

    public enum RoomDailyStatus
    {
        Free, Booked, Debt
    }

    public class RoomInfo
    {
        public int RoomNumber { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public RoomType Type { get; set; }
        public int Floor { get; set; }
        public ObservableCollection<RoomDailyInfo> DailyInfo { get; set; }
    }

    public class RoomDailyInfo
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public RoomDailyStatus Status { get; set; }
    }

}
