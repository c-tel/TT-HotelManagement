﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTHotel.Contracts.Auth;

namespace TTHotel.API.Services
{
    public class InMemoryAuthService : IAuthService
    {
        private readonly Dictionary<string, UserDTO> _storage = new Dictionary<string, UserDTO>();

        public string Authorize(UserDTO user)
        {
            var sessID = Guid.NewGuid().ToString();
            _storage.Add(sessID, user);
            return sessID;
        }

        public UserDTO GetAuthorized(string sessID)
        {
            return _storage.GetValueOrDefault(sessID);
        }
    }
}
