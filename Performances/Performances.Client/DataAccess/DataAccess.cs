using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Performances.Api.Models;
using Performances.Model;

namespace Performances.Client.DataAccess
{
    public static class DataAccess
    {
        private readonly static HttpClient _client;

        static DataAccess()
        {
            _client = new HttpClient {BaseAddress = new Uri("http://localhost:54860/") };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region UserMethods

        public static async Task<User> LoginUser(string Email, string Password)
        {
            var path = "api/user" + Email + "/" + Password;
            var response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<User>();
            }
            return null;
        }

        public static async Task<User> CreateUser(CreateUserParameters userParam)
        {
            HttpResponseMessage response;
            response = await _client.PostAsJsonAsync("api/user",
                new CreateUserParameters()
                {
                    photo = userParam.photo,
                    user = userParam.user,
                });
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<User>();
            }
            return null;
        }

        #endregion

        #region CreativeTeamMethods

        public static async Task<CreativeTeam> CreateCreativeTeam(CreateCreativeTeamParameters creativeTeamParam)
        {

            HttpResponseMessage response;
            response = await _client.PostAsJsonAsync("api/creativeteam",
                new CreateCreativeTeamParameters()
                {
                    photo = creativeTeamParam.photo,
                    creativeTeam = creativeTeamParam.creativeTeam,
                    filename = creativeTeamParam.filename
                });
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<CreativeTeam>();
            }
            return null;
        }

        #endregion


    }
}
