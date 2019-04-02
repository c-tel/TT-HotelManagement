using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using TTHohel.Contracts.Bookings;
using TTHohel.Models;
using TTHotel.Contracts.Auth;
using TTHotel.Contracts.Bookings;

namespace TTHohel.Services
{
    class HotelApiClient
    {
        private HttpClient Client;
        private static HotelApiClient ApiClient;

        public User User { get; private set; }

        private HotelApiClient()
        {
            Client = new HttpClient
            {
                //BaseAddress = new Uri("https://tt-hotel.herokuapp.com/api")
                BaseAddress = new Uri("https://localhost:44358/api")
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

        private string ToQueryArgument(DateTime dateTime)
        {
            return $"{dateTime.Month}%2F{dateTime.Day}%2F{dateTime.Year}";
        }

    }
}
