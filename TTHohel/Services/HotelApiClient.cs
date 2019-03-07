using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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

    }
}
