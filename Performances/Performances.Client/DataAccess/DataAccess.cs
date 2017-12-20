using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
    }
}
