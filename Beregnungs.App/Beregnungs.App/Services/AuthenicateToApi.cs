using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Beregnungs.App.Models;
using Newtonsoft.Json;

namespace Beregnungs.App.Services
{
    class AuthenicateToApi : IAuthenticate
    {
        HttpClient _client;
        string url = $"api/schlaege";


        public AuthenicateToApi()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"{App.RESTBackendURL}/");
        }

        public async Task<bool> Login(Account account)
        {
            if (account == null)
            {
                return false;
            }

            var serializedItem = JsonConvert.SerializeObject(account);
            var response = await _client.PostAsync($"api/account/login", new StringContent(serializedItem, Encoding.UTF8, "application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "your_authorization_token_string");
            var result = response.Content.ReadAsStringAsync().Result;
            result =  result.Substring(10, 280);
            App.Token = result;
            return response.IsSuccessStatusCode;
        }
    }
}
