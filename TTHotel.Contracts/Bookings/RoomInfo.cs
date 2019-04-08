using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TTHohel.Contracts.Bookings
{
    public enum RoomType
    {
        Standard,
        Econom,
        Luxe,
        Studio,
        Apartments
    }

    public enum RoomDailyStatus
    {
        Free, Booked, Settled
    }

    public class RoomInfo
    {
        public int RoomNumber { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public RoomType Type { get; set; }
        public int Floor { get; set; }
        public List<RoomDailyInfo> DailyInfo { get; set; }
    }

    public class RoomDailyInfo
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public RoomDailyStatus Status { get; set; }
        public double? Debt { get; set; }
        public int? BookID { get; set; }
        public DateTime BookDate { get; set; }
    }

}
