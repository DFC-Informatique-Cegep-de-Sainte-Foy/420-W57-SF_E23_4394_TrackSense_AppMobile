﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TrackSense.Configurations;
using TrackSense.Entities;
using TrackSense.Services.API.APIDTO;

namespace TrackSense.Services
{
    public class UserService
    {
        HttpClient httpClient;
        IConfigurationManager _config;

        public UserService(IConfigurationManager config)
        {
            httpClient = new HttpClient();
            _config = config;
        }

        public async Task<HttpResponseMessage> CreateUser(User user)
        {
            Settings userSettings = _config.LoadSettings();
            string url = $"{userSettings.ApiUrl}Authentication/SignUp";

            UserDTO userDTO = new UserDTO(user);

            return await httpClient.PostAsJsonAsync(url, userDTO);
        }

    }
}
