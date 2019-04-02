using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TTHotel.API.Services;
using TTHotel.Contracts.Auth;

namespace TTHotel.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private IHotelService _hotelService;
        public AuthController(IAuthService authService, IHotelService hotelService)
        {
            _authService = authService;
            _hotelService = hotelService;
        }

        [HttpPost("authorize")]
        public ActionResult<AuthorizationResponse> Authorize(Credentials creds)
        {
            var user = _hotelService.GetUser(creds.Login, creds.Password);
            if(user == null)
                return Unauthorized();
            var sessid = _authService.Authorize(user);
            Response.Cookies.Append("sessid", sessid);
            return new AuthorizationResponse
            {
                SessionKey = sessid,
                User = user
            };
        }

    }
}