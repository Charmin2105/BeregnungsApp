using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Beregnungs.App.Models;
using Newtonsoft.Json;

namespace Beregnungs.App.Services
{
    public class AuthenicateToApi : IAuthenticate
    {
        private HttpClient _client;
        private string url = $"api/account/login";

        //Ctor
        public AuthenicateToApi()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"{App.RESTBackendURL}/");
        }

        //Login
        public async Task<bool> Login(Account account)
        {
            if (account == null)
            {
                return false;
            }
            // Anmelde daten in JSON wandeln
            var serializedItem = JsonConvert.SerializeObject(account);
            // An Server senden
            var response = await _client.PostAsync(url, new StringContent(serializedItem, Encoding.UTF8, "application/json"));
            // Antwort des Servers als String Wandeln
            var result = response.Content.ReadAsStringAsync().Result;

            // Token aus der Antwort extrahieren
            var token =  result.Substring(10, 280);

            //BetriebID aus der Antwort extrahieren
            var betriebID = result.Substring(305, 36);

            //App und Token setzen
            App.Token = token;
            App.BetriebID = betriebID;

            return response.IsSuccessStatusCode;
        }
    }
}
