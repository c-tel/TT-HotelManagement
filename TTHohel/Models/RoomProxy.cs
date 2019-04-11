using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTHohel.Contracts.Bookings;
using TTHotel.Contracts.Rooms;

namespace TTHohel.Models
{
    public class RoomProxy
    {
        public string Num { get; set; }
        public string Floor { get; set; }
        public string Places { get; set; }
        public string Price { get; set; }
        public RoomType Type { get; set; }

        public RoomProxy() { }

        public RoomProxy(RoomDTO room)
        {
            Num = room.Num == 0 ? null : room.Num.ToString();
            Floor = room.Floor == 0 ? null : room.Floor.ToString();
            Places = room.Places == 0 ? null : room.Places.ToString();
            Price = room.Price == 0 ? null : room.Price.ToString();
            Type = room.Type;
        }

        public RoomDTO Convert()
        {
            var num_valid = int.TryParse(Num, out var num);
            var floor_valid = int.TryParse(Floor, out var floor);
            var places_valid = int.TryParse(Places, out var places);
            var price_valid = double.TryParse(Price, out var price);
            if (!num_valid || !floor_valid || !places_valid || !price_valid || price <= 0 || floor <= 0 || num <= 0)
                return null;
            return new RoomDTO
            {
                Floor = floor,
                Num = num,
                Places = places,
                Price = price,
                Type = Type
            };
        }
    }
}
