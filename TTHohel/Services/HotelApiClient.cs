using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using TTHohel.Contracts.Bookings;
using TTHohel.Models;
using TTHotel.Contracts.Auth;
using TTHotel.Contracts.Bookings;
using TTHotel.Contracts.Cleanings;
using TTHotel.Contracts.Clients;
using TTHotel.Contracts.Payments;
using TTHotel.Contracts.Rooms;

namespace TTHohel.Services
{
    class HotelApiClient
    {
        private HttpClient Client;
        private HttpClient LocalClient;
        private static HotelApiClient ApiClient;

        public User User { get; private set; }

        private HotelApiClient()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri("https://tt-hotel.herokuapp.com/api")
                //BaseAddress = new Uri("https://localhost:44358/api")
            };
            LocalClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44358/api")
                //BaseAddress = new Uri("https://localhost:44358/api")
            };
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static HotelApiClient GetInstance()
        {
            if (ApiClient == null)
                ApiClient = new HotelApiClient();
            return ApiClient;
        }

        public AuthorizationResponse Login(string login, string password)
        {
            var userCredentials = new Credentials
            {
                Login = login,
                Password = password
            };

            var resp = Client.PostAsJsonAsync("api/auth/authorize", userCredentials).Result;
            if (!resp.IsSuccessStatusCode)
                return null;
            return resp.Content.ReadAsAsync<AuthorizationResponse>().Result;
        }

        public List<RoomInfo> RoomInfos(DateTime from, DateTime to)
        {
            UriBuilder builder = new UriBuilder("https://tt-hotel.herokuapp.com/api/bookings")
            //UriBuilder builder = new UriBuilder("https://localhost:44358/api/bookings")
            {
                Query = $"from={ToQueryArgument(from)}&to={ToQueryArgument(to)}"
            };
            
            var resp = Client.GetAsync(builder.Uri).Result;
            using (HttpContent content = resp.Content)
            {
                var result = content.ReadAsAsync<List<RoomInfo>>().Result;

                return result;
            }
        }

        public BookingDTO GetBookingById(int bookingId)
        {
            var resp = Client.GetAsync($"api/bookings/{bookingId}").Result;
            if (!resp.IsSuccessStatusCode)
                return null;
            return resp.Content.ReadAsAsync<BookingDTO>().Result;
        }

        public bool SetBookingStatus(int bookingId, BookingStates state)
        {
            var route = "";
            switch (state)
            {
                case BookingStates.Settled:
                    route = "settle";
                    break;
                case BookingStates.Canceled:
                    route = "cancel";
                    break;
                case BookingStates.Non_settle:
                    route = "close";
                    break;
                default:
                    return false;
            }

            var resp = Client.PutAsync($"api/bookings/{bookingId}/{route}", null).Result;

            return resp.IsSuccessStatusCode;
        }

        public bool EditBooking(BookingDTO booking)
        {
            var newBooking = new BookingUpdateDTO
            {
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                BookComment = booking.BookComment,
                Complaint = booking.Complaint
            };

            var resp = Client.PutAsJsonAsync($"api/bookings/{booking.BookingId}", newBooking).Result;

            return resp.IsSuccessStatusCode;
        }

        public List<ClientDTO> GetAllClients()
        {
            var resp = Client.GetAsync($"api/clients").Result;
            if (!resp.IsSuccessStatusCode)
                return null;
            return resp.Content.ReadAsAsync<List<ClientDTO>>().Result;
        }

        public List<CleaningDTO> GetAllCleanings()
        {
            var resp = Client.GetAsync($"api/cleanings").Result;
            if (!resp.IsSuccessStatusCode)
                return null;
            return resp.Content.ReadAsAsync<List<CleaningDTO>>().Result;
        }
        public List<CleaningStatsDTO> GetCleaningsStats(DateTime date)
        {
            var resp = Client.GetAsync($"api/cleanings/statistics?date={ToQueryArgument(date)}").Result;
            if (!resp.IsSuccessStatusCode)
                return null;
            return resp.Content.ReadAsAsync<List<CleaningStatsDTO>>().Result;
        }

        public List<ClientAnalisedDTO> GetAnalisedClients()
        {
            var resp = Client.GetAsync($"api/clients/analytics").Result;
            if (!resp.IsSuccessStatusCode)
                return null;
            return resp.Content.ReadAsAsync<List<ClientAnalisedDTO>>().Result;
        }

        public List<ClientDTO> GetAnalisedSuitClients()
        {
            var resp = Client.GetAsync($"api/clients/suit").Result;
            if (!resp.IsSuccessStatusCode)
                return null;
            return resp.Content.ReadAsAsync<List<ClientDTO>>().Result;
        }

        public ClientDTO GetClient(string tel_num)
        {
            var route = $"api/clients/{tel_num.TrimStart('+')}";

            var resp = Client.GetAsync(route).Result;
            if (!resp.IsSuccessStatusCode)
                return null;

            return resp.Content.ReadAsAsync<ClientDTO>().Result;
        }

        public System.Net.HttpStatusCode CreateClient(ClientDTO clientDTO)
        {
            var resp = Client.PostAsJsonAsync("api/clients", clientDTO).Result;

            return resp.StatusCode;
        }
        public bool CreateCleaning(CleaningDTO cleaning)
        {
            var resp = Client.PostAsJsonAsync("api/cleanings", cleaning).Result;

            return resp.IsSuccessStatusCode;
        }

        public System.Net.HttpStatusCode CreateRoom(RoomDTO roomDTO)
        {
            var room = new RoomCreateDTO
            {
                Num = roomDTO.Num,
                Floor = roomDTO.Floor,
                Places = roomDTO.Places,
                Price = roomDTO.Price,
                Type = roomDTO.Type
            };
            var resp = Client.PostAsJsonAsync("api/rooms", room).Result;

            return resp.StatusCode;
        }

        public RoomDTO GetRoom(int roomNumber)
        {
            var route = $"api/rooms/{roomNumber}";

            var resp = Client.GetAsync(route).Result;
            if (!resp.IsSuccessStatusCode)
                return null;

            return resp.Content.ReadAsAsync<RoomDTO>().Result;

        }

        public List<RoomDTO> GetFreeRooms(DateTime from, DateTime to, int places)
        {
            var route = $"api/rooms?from={ToQueryArgument(from)}&to={ToQueryArgument(to)}&guests={places}";
            var resp = Client.GetAsync(route).Result;
            if (!resp.IsSuccessStatusCode)
                return null;
            return resp.Content.ReadAsAsync<List<RoomDTO>>().Result;
        }

        public List<RoomStatisticsDTO> GetRoomsStats(DateTime from, DateTime to)
        {
            var route = $"api/rooms/statistics?from={ToQueryArgument(from)}&to={ToQueryArgument(to)}";
            var resp = Client.GetAsync(route).Result;
            if (!resp.IsSuccessStatusCode)
                return null;
            return resp.Content.ReadAsAsync<List<RoomStatisticsDTO>>().Result;
        }

        public System.Net.HttpStatusCode UpdateClient(ClientDTO clientDTO, string oldNum)
        {
            var resp = Client.PutAsJsonAsync($"api/clients/{oldNum.TrimStart('+')}", clientDTO).Result;

            return resp.StatusCode;
        }

        public System.Net.HttpStatusCode CreatePersonnel(UserCreateDTO user)
        {
            var resp = Client.PostAsJsonAsync("api/auth", user).Result;

            return resp.StatusCode;
        }

        public bool CreateBooking(DateTime from, DateTime to, int bookedRoomNum, string clientTel, string bookComment)
        {
            var booking = new BookingCreateDTO
            {
                StartDate = from.AddHours(3),
                EndDate = to.AddHours(3),
                BookComment = bookComment,
                BookedRoomNum = bookedRoomNum,
                ClientTel = clientTel
            };

            var resp = Client.PostAsJsonAsync("api/bookings", booking).Result;

            return resp.IsSuccessStatusCode;
        }

        public bool AddPayment(int bookingId, PaymentTypes paymentType, double amount)
        {
            var payment = new PaymentCreateDTO
            {
                Type = paymentType,
                Amount = amount
            };

            var resp = Client.PostAsJsonAsync($"api/bookings/{bookingId}/payments", payment).Result;

            return resp.IsSuccessStatusCode;
        }

        public List<ReportItem> GetReport(DateTime asOfDate)
        {
            var route = $"/api/bookings/report?date={ToQueryArgument(asOfDate)}";
            var resp = Client.GetAsync(route).Result;
            if (!resp.IsSuccessStatusCode)
                return null;
            return resp.Content.ReadAsAsync<List<ReportItem>>().Result;
        }


        private string ToQueryArgument(DateTime dateTime)
        {
            return $"{dateTime.Month}%2F{dateTime.Day}%2F{dateTime.Year}";
        }

    }
}
