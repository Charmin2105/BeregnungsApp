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
    class BeregnungsDatenRESTStore : IDataStore<BeregnungsDaten>
    {
        HttpClient _client;
        IEnumerable<BeregnungsDaten> beregnungsDatens;


        public BeregnungsDatenRESTStore()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"{App.RESTBackendURL}/");
            beregnungsDatens = new List<BeregnungsDaten>();
        }

        public async Task<bool> AddDatenAsync(BeregnungsDaten daten)
        {
            if (daten == null)
            {
                return false;
            }
            var serializedItem = JsonConvert.SerializeObject(daten);
            var response = await _client.PostAsync($"api/betriebe/25320c5e-f58a-4b1f-b63a-8ee07a840bdf/beregnungsdaten",new StringContent(serializedItem,Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        public Task<BeregnungsDaten> GetDatenAsync(Guid id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<BeregnungsDaten>> GetDatensAsync(bool forceRefresh = false)
        {
            beregnungsDatens = null;

            if (forceRefresh)
            {
                var response = await _client.GetAsync($"api/betriebe/25320c5e-f58a-4b1f-b63a-8ee07a840bdf/beregnungsdaten");
                var json = await response.Content.ReadAsStringAsync();
                beregnungsDatens = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<BeregnungsDaten>>(json));

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\tBeregnungsDaten erfolgreich geladen.");
                }

            }

            return beregnungsDatens;
        }
        public async Task<bool> DeleteDatenAsync(Guid id)
        {
            var response = await _client.DeleteAsync($"api/betriebe/25320c5e-f58a-4b1f-b63a-8ee07a840bdf/beregnungsdaten/{id}");

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateDatenAsync(BeregnungsDaten daten)
        {
            if (daten == null)
            {
                return false;
            }

            var serializedItem = JsonConvert.SerializeObject(daten);
            var response = await _client.PutAsync($"api/betriebe/25320c5e-f58a-4b1f-b63a-8ee07a840bdf/beregnungsdaten/{daten.ID.ToString()}", new StringContent(serializedItem, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }


    }
}
