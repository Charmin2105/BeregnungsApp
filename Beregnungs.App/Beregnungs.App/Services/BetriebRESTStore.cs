using Beregnungs.App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Beregnungs.App.Services
{
    class BetriebRESTStore : IDataStore<Betrieb>
    {
        HttpClient _client;
        IEnumerable<Betrieb> betriebe;

        string url = $"api/betriebe";

        public BetriebRESTStore()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"{App.RESTBackendURL}/");
            betriebe = new List<Betrieb>();
        }

        #region Methods
        public async Task<bool> AddDatenAsync(Betrieb daten)
        {
            if (daten == null)
            {
                return false;
            }
            var serializedItem = JsonConvert.SerializeObject(daten);
            var response = await _client.PostAsync(url, new StringContent(serializedItem, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteDatenAsync(Guid id)
        {
            var response = await _client.DeleteAsync($"{url}/{id}");
            return response.IsSuccessStatusCode;
        }

        public Task<Betrieb> GetDatenAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Betrieb>> GetDatensAsync(bool forceRefresh = false)
        {
            betriebe = null;

            if (forceRefresh)
            {
                var response = await _client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                betriebe = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Betrieb>>(json));
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\t Betrieb erfolgreich geladen.");
                }
            }
            return betriebe;
        }

        public async Task<bool> UpdateDatenAsync(Betrieb daten)
        {
            if (daten == null)
            {
                return false;
            }

            var serializedItem = JsonConvert.SerializeObject(daten);
            var response = await _client.PutAsync($"{url}/{daten.ID.ToString()}", new StringContent(serializedItem, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        } 
        #endregion
    }
}
