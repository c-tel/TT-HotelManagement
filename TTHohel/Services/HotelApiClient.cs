using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using TTHohel.Contracts.Bookings;
using TTHohel.Models;
using TTHotel.Contracts.Auth;

namespace TTHohel.Services
{
    class HotelApiClient
    {
        public AuthorizationResponse AuthorizationResponse { get; private set; }
        private HttpClient Client;
        private static HotelApiClient ApiClient;

        public event Action<User> UserChanged;
        public User User { get; private set; }

        private HotelApiClient()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri("https://tt-hotel.herokuapp.com/api")
            };
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void ChangeUser(User user)
        {
            User = user;
            UserChanged?.Invoke(user);
        }

        public static HotelApiClient GetInstance()
        {
            if (ApiClient == null)
                ApiClient = new HotelApiClient();
            return ApiClient;
        }

        public bool Login(string login, string password)
        {
            var userCredentials = new Credentials
            {
                Login = login,
                Password = password
            };

            var resp = Client.PostAsJsonAsync("api/auth/authorize", userCredentials).Result;
            if (!resp.IsSuccessStatusCode)
                return false;
            AuthorizationResponse = resp.Content.ReadAsAsync<AuthorizationResponse>().Result;
            return true;
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
        private string ToQueryArgument(DateTime dateTime)
        {
            return dateTime.Ticks.ToString();
        }

    }
}
