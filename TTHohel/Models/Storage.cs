using System;
using TTHotel.Contracts.Bookings;

namespace TTHohel.Models
{
    public class Storage
    {
        private static Storage _instance;

        public event Action<BookingDTO> BookingChanged;
        public event Action<User> UserChanged;
        public event Action BookingsChanged;

        public BookingDTO SelectedBooking { get; private set; }
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

        public void ChangeBooking(BookingDTO nextBooking)
        {
            SelectedBooking = nextBooking;
            BookingChanged?.Invoke(SelectedBooking);
        }

        public void ChangeBookings()
        {
            BookingsChanged?.Invoke();
        }
    }
}
