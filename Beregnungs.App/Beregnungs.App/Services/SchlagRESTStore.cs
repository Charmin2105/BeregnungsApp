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
    class SchlagRESTStore : IDataStore<Schlag>
    {
        HttpClient _client;
        IEnumerable<Schlag> schlaege;
        private string betriebID = App.BetriebID;


        /// <summary>
        /// Ctor
        /// </summary>
        public SchlagRESTStore()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"{App.RESTBackendURL}/");
            //Authentication Header dem Http Client hinzufügen 
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            schlaege = new List<Schlag>();
        }

        #region Methods
        /// <summary>
        /// Methode zum hinzufügen von einem Schlag
        /// </summary>
        /// <param name="daten">Schlag</param>
        /// <returns>bool</returns>
        public async Task<bool> AddDatenAsync(Schlag daten)
        {
            //Prüfen ob übergabe Parameter null ist
            if (daten == null)
            {
                return false;
            }
            //In JSON umwandeln
            var serializedItem = JsonConvert.SerializeObject(daten);

            //An Server senden
            var response = await _client.PostAsync($"api/betriebe/" + betriebID + "/schlaege", new StringContent(serializedItem, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Methode zum löschen eines Schlags
        /// </summary>
        /// <param name="id">Id des Schlags</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteDatenAsync(Guid id)
        {
            //Anfrage an Server
            var response = await _client.DeleteAsync($"api/betriebe/" + betriebID + "/schlaege/{id}");

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Laden aller Schläge
        /// </summary>
        /// <param name="forceRefresh">neu laden</param>
        /// <returns>IEnumerable<Schlag></returns>
        public async Task<IEnumerable<Schlag>> GetDatensAsync(bool forceRefresh = false)
        {
            //Variabel schlaege auf null setzen
            schlaege = null;

            if (forceRefresh)
            {
                //Anfrage an Server
                var response = await _client.GetAsync($"api/betriebe/" + betriebID + "/schlaege");

                //Umwandeln der JSON in IEnumerable<Schlag>
                var json = await response.Content.ReadAsStringAsync();
                schlaege = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Schlag>>(json));

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\t Schläge erfolgreich geladen.");
                }

            }

            return schlaege;
        }

        /// <summary>
        /// Methode zum laden eines einzelnen Schlag
        /// Nicht implementiert
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Schlag> GetDatenAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Methode zum Updaten von einem Schlag
        /// </summary>
        /// <param name="daten">Schlag</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateDatenAsync(Schlag daten)
        {
            //Prüfen ob übergabe Parameter leer ist
            if (daten == null)
            {
                return false;
            }

            //In JSON umwandeln
            var serializedItem = JsonConvert.SerializeObject(daten);

            //An Server senden
            var response = await _client.PutAsync($"api/betriebe/" + betriebID + "/schlaege/{daten.ID.ToString()}", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }
        #endregion
    }
}
