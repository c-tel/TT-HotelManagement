using Microsoft.AspNetCore.Mvc;
using TTHotel.API.Services;
using TTHotel.Contracts.Auth;

namespace TTHotel.API.Controllers
{
    // some other bla
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
            var sess = _authService.Authorize(user);
            return new AuthorizationResponse
            {
                SessionKey = sess,
                User = user
            };
        }

    }
}