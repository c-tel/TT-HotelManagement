﻿using TTHotel.Contracts.Clients;

namespace TTHohel.Models
{
    public class ClientDisplayData
    {
        public ClientDTO Client { get; set; }
        public ClientViewModes Mode { get; set; }
        public ModesEnum CameFrom { get; set; }
    }
}
