using System;
using TTHotel.Contracts.Bookings;
using TTHotel.Contracts.Clients;

namespace TTHohel.Models
{
    public class Storage
    {
        private static Storage _instance;

        public event Action<BookingDTO> BookingChanged;
        public event Action<User> UserChanged;
        public event Action BookingsChanged;
        public event Action<ClientDTO> AllClientsChanged;

        public event Action<ClientDisplayData> ClientDisplayChanged;

        public BookingDTO SelectedBooking { get; private set; }
        public RoomDisplayData RoomData { get; private set; }
        public User User { get; private set; }

        public static Storage Instance { get => GetInstance();  }

        private Storage() { }

        private static Storage GetInstance()
        {
            if (_instance == null)
                _instance = new Storage();
            return _instance;
        }

        public void ChangeUser(User user)
        {
            User = user;
            UserChanged?.Invoke(user);
        }

        //public void ChangeRoom(BookingDTO nextBooking)
        //{
        //    SelectedBooking = nextBooking;
        //    BookingChanged?.Invoke(SelectedBooking);

        //    ChangeBookings();
        //}

        public void ChangeBooking(BookingDTO nextBooking)
        {
            SelectedBooking = nextBooking;
            BookingChanged?.Invoke(SelectedBooking);

            ChangeBookings();
        }

        public void ChangeBookings()
        {
            BookingsChanged?.Invoke();
        }

        public void ChangeAllClients(ClientDTO clientDTO)
        {
            AllClientsChanged?.Invoke(clientDTO);
        }

        public void ChangeClientDisplayData(ClientDisplayData data)
        {
            ClientDisplayChanged?.Invoke(data);
        }

        public void ChangeRoomDisplayData(RoomDisplayData data)
        {
            RoomData = data;
            ChangeBookings();
        }
    }
}
