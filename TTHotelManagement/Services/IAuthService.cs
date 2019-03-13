using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTHotel.Contracts.Auth;

namespace TTHotel.API.Services
{
    public interface IAuthService
    {
        string Authorize(UserDTO user);
        UserDTO GetAuthorized(string sessID);
    }
}
