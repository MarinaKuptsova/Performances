using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
    }
}
