using Beregnungs.App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Beregnungs.App.Services
{
    class BeregnungsDatenRESTStore : IDataStore<BeregnungsDaten>
    {
        HttpClient _client;
        IEnumerable<BeregnungsDaten> beregnungsDatens;
        string betriebID = App.BetriebID;

        /// <summary>
        /// Ctor
        /// </summary>
        public BeregnungsDatenRESTStore()
        {
            //Http Client erstellen
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"{App.RESTBackendURL}/");
            //Authentication Header dem Http Client hinzufügen 
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            beregnungsDatens = new List<BeregnungsDaten>();
        }

        /// <summary>
        /// Methode zum Daten hinzufügen
        /// </summary>
        /// <param name="daten">zu übergebene Daten</param>
        /// <returns>bool</returns>
        public async Task<bool> AddDatenAsync(BeregnungsDaten daten)
        {
            //Prüfen ob übergabeparameter leer ist
            if (daten == null)
            {
                return false;
            }
            //In JSON Format konvertieren
            var serializedItem = JsonConvert.SerializeObject(daten);
            //Daten übertragen
            var response = await _client.PostAsync($"api/betriebe/" + betriebID + "/beregnungsdaten",
                new StringContent(serializedItem, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Methode zum laden einer bestimmten Beregnungsdaten
        /// Nicht Implementiert
        /// </summary>
        /// <param name="id">Id des zu gesuchten Betrieb</param>
        /// <returns>BeregnungsDaten</returns>
        public Task<BeregnungsDaten> GetDatenAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Methode zum laden aller Beregnungsdaten eines Betriebs
        /// </summary>
        /// <param name="forceRefresh"> neu laden</param>
        /// <returns><IEnumerable<BeregnungsDaten>></returns>
        public async Task<IEnumerable<BeregnungsDaten>> GetDatensAsync(bool forceRefresh = false)
        {
            //Variabel beregnungsDatens auf null setzten
            beregnungsDatens = null;

            if (forceRefresh)
            {
                //Anfrage an Server
                var response = await _client.GetAsync($"api/betriebe/" + betriebID + "/beregnungsdaten");
                //Anfrage umwandeln
                var json = await response.Content.ReadAsStringAsync();
                beregnungsDatens = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<BeregnungsDaten>>(json));

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\tBeregnungsDaten erfolgreich geladen.");
                }

            }

            return beregnungsDatens;
        }

        /// <summary>
        /// Methode zum löschen von Beregnungsdaten
        /// </summary>
        /// <param name="id">Id der zu löschenden Daten</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteDatenAsync(Guid id)
        {
            //Anfrage an Server
            var response = await _client.DeleteAsync($"api/betriebe/" + betriebID + "/beregnungsdaten/{id}");

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Methode zum Update von Beregnungsdaten
        /// </summary>
        /// <param name="daten">Geänderte Daten</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateDatenAsync(BeregnungsDaten daten)
        {
            //Prüfen ob übergabeparameter leer ist
            if (daten == null)
            {
                return false;
            }

            //In JSON konvertieren
            var serializedItem = JsonConvert.SerializeObject(daten);
            //An Server senden
            var response = await _client.PutAsync($"api/betriebe/" + betriebID + "/beregnungsdaten/{daten.ID.ToString()}", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }


    }
}
