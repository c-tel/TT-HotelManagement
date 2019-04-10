using TTHotel.Contracts.Rooms;

namespace TTHohel.Models
{
    public class RoomDisplayData
    {
        public RoomDTO Room { get; set; }
        public DisplayModes Mode { get; set; }
        public ModesEnum CameFrom { get; set; }
    }
}
