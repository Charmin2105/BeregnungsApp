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
    class BetriebRESTStore : IDataStore<Betrieb>
    {
        HttpClient _client;
        IEnumerable<Betrieb> betriebe;

        string url = $"api/betriebe";

        /// <summary>
        /// Ctor
        /// </summary>
        public BetriebRESTStore()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"{App.RESTBackendURL}/");
            //Authentication Header dem Http Client hinzufügen 
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            betriebe = new List<Betrieb>();
        }

        #region Methods
        /// <summary>
        /// Methode zum hinzufügen von Betrieben
        /// </summary>
        /// <param name="daten">Betrieb</param>
        /// <returns>bool</returns>
        public async Task<bool> AddDatenAsync(Betrieb daten)
        {
            //Prüfen ob übergabeparameter null ist
            if (daten == null)
            {
                return false;
            }
            //In JSON konvertieren
            var serializedItem = JsonConvert.SerializeObject(daten);
            //An Server senden
            var response = await _client.PostAsync(url, new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Methode zum löschen von Betrieben
        /// </summary>
        /// <param name="id">Id des Betriebs</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteDatenAsync(Guid id)
        {
            //Anfrage an Server
            var response = await _client.DeleteAsync($"{url}/{id}");

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Methode zum laden eines Bestimmten Betriebs
        /// Nicht implementiert
        /// </summary>
        /// <param name="id">Id des Betriebs</param>
        /// <returns>Betrieb</returns>
        public Task<Betrieb> GetDatenAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Methoden zum laden aller Betriebe
        /// </summary>
        /// <param name="forceRefresh">Neuladen</param>
        /// <returns>IEnumerable<Betrieb></returns>
        public async Task<IEnumerable<Betrieb>> GetDatensAsync(bool forceRefresh = false)
        {
            //Variable betriebe auf null setzen
            betriebe = null;

            if (forceRefresh)
            {
                //Anfrage an Server
                var response = await _client.GetAsync(url);
                //JSON umwandeln
                var json = await response.Content.ReadAsStringAsync();
                betriebe = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Betrieb>>(json));

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\t Betrieb erfolgreich geladen.");
                }
            }
            return betriebe;
        }

        /// <summary>
        /// Methode zum Updaten eines Betriebs
        /// </summary>
        /// <param name="daten">Betrieb</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateDatenAsync(Betrieb daten)
        {
            //Prüfen ob übergabe Parameter leer ist
            if (daten == null)
            {
                return false;
            }

            //In JSON umwandeln
            var serializedItem = JsonConvert.SerializeObject(daten);
            //An Server senden
            var response = await _client.PutAsync($"{url}/{daten.ID.ToString()}", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        } 
        #endregion
    }
}
