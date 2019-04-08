using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TTHotel.Contracts.Cleanings
{
    public enum CleaningTypes
    {
        [Description("Планове")]
        Common,
        [Description("Зміна білизни")]
        WithLinen,
        [Description("Виїзд")]
        Unsettle
    }

    public class CleaningDTO
    {
        public CleaningTypes Type { get; set; }
        public int RoomNum { get; set; }
    }
}
