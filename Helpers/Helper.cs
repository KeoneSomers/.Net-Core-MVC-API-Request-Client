using System;
using System.Net.Http;

namespace apiRequest.Helpers
{
    public class StudentAPI
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:6001");
            return client;
        }
    }
}