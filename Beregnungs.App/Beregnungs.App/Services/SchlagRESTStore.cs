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
    class SchlagRESTStore : IDataStore<Schlag>
    {
        HttpClient _client;
        IEnumerable<Schlag> schlaege;

        string url = $"api/schlaege";


        public SchlagRESTStore()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"{App.RESTBackendURL}/");
            //client.BaseAddress = new Uri($"{App.AzureBackendUrl}/");
            schlaege = new List<Schlag>();
        }

        #region Methods
        public async Task<bool> AddDatenAsync(Schlag daten)
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
            var response = await _client.DeleteAsync($"api/schlaege/{id}");
            return response.IsSuccessStatusCode;
        }
        public async Task<IEnumerable<Schlag>> GetDatensAsync(bool forceRefresh = false)
        {
            schlaege = null;

            if (forceRefresh)
            {
                var response = await _client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                schlaege = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Schlag>>(json));

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\t Schläge erfolgreich geladen.");
                }

            }

            return schlaege;
        }
        public Task<Schlag> GetDatenAsync(Guid id)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> UpdateDatenAsync(Schlag daten)
        {
            if (daten == null)
            {
                return false;
            }

            var serializedItem = JsonConvert.SerializeObject(daten);
            var response = await _client.PutAsync($"api/schlaege/{daten.ID.ToString()}", new StringContent(serializedItem, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        } 
        #endregion
    }
}
