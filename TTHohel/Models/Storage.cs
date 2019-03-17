using System;
using TTHotel.Contracts.Bookings;

namespace TTHohel.Models
{
    public class Storage
    {
        private static Storage _instance;
        private static Storage GetInstance()
        {
            if (_instance == null)
                _instance = new Storage();
            return _instance;
        }
        public static Storage Instance { get => GetInstance();  }
        private Storage() { }

        public event Action<User> UserChanged;
        public User User { get; private set; }

        public void ChangeUser(User user)
        {
            User = user;
            UserChanged?.Invoke(user);
        }

        public event Action<BookingDTO> BookingChanged;
        public BookingDTO SelectedBooking { get; private set; }

        public void ChangeBooking(BookingDTO nextBooking)
        {
            SelectedBooking = nextBooking;
            BookingChanged?.Invoke(SelectedBooking);
        }
    }
}
