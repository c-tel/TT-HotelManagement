using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TTHohel.Contracts.Bookings;

namespace TTHohel.Services
{
    class HotelApiClient
    {
        private string SessionId;
        private HttpClient Client;
        private static HotelApiClient ApiClient;

        private HotelApiClient()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri("https://tt-hotel.herokuapp.com/api")
            };
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static HotelApiClient GetInstance()
        {
            if (ApiClient == null)
                ApiClient = new HotelApiClient();
            return ApiClient;
        }
        public List<RoomInfo> RoomInfos(DateTime from, DateTime to)
        {
            UriBuilder builder = new UriBuilder("http://localhost:6598/api/bookings")
            {
                Query = $"from='{ToQueryArgument(from)}'&to='{ToQueryArgument(to)}'"
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
            return $"{dateTime.Month}%2F{dateTime.Day}%2F{dateTime.Year}";
        }

    }
}
