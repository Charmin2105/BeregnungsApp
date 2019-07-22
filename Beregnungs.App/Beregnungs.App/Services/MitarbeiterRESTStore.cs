using Beregnungs.App.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Beregnungs.App.Services
{
    public class MitarbeiterRESTStore : IDataStore<Mitarbeiter>
    {
        //Ctor
        private HttpClient _client;
        public IEnumerable<Mitarbeiter> mitarbeiters;
        private string betriebID = App.BetriebID;

        //Ctor
        public MitarbeiterRESTStore()
        {
            //Http Client erstellen
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"{App.RESTBackendURL}/");
            //Authentication Header dem Http Client hinzufügen 
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            mitarbeiters = new List<Mitarbeiter>();
        }

        /// <summary>
        /// Mitarbeiter erstellen
        /// </summary>
        /// <param name="daten">Neuer Mitarbeiter</param>
        /// <returns>statuscode</returns>
        public async Task<bool> AddDatenAsync(Mitarbeiter daten)
        {
            //Prüfen ob Daten null sind
            if (daten == null)
            {
                return false;
            }
            //In JSON konvertieren
            var serializedItem = JsonConvert.SerializeObject(daten);

            //Daten übertragen
            var response = await _client.PostAsync($"api/betriebe/" + betriebID + "/mitarbeiters",
                new StringContent(serializedItem, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                DependencyService.Get<IMessage>().LongAlert("Mitarbeiter erfolgreich gespeichert");

            }
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Mitarbeiter löschen
        /// </summary>
        /// <param name="id">Id des Mitarbeiters</param>
        /// <returns>Statuscode</returns>
        public async Task<bool> DeleteDatenAsync(Guid id)
        {
            //Anfrage an Server
            var response = await _client.DeleteAsync($"api/betriebe/" + betriebID + "/mitarbeiters/" + id);
            return response.IsSuccessStatusCode;
        }

        public Task<Mitarbeiter> GetDatenAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Alle Mitarbeiter eines Betriebs laden
        /// </summary>
        /// <param name="forceRefresh">neu laden</param>
        /// <returns>Mitarbeiter</returns>
        public async Task<IEnumerable<Mitarbeiter>> GetDatensAsync(bool forceRefresh = false)
        {
            //Variabel mitarbeiters auf null setzten
            mitarbeiters = null;
            if (forceRefresh)
            {
                //Anfrage an Server
                var response = await _client.GetAsync($"api/betriebe/" + betriebID + "/mitarbeiters");
                //Anfrage umwandeln
                var json = await response.Content.ReadAsStringAsync();
                var parsedObject = JObject.Parse(json);
                var popupJson = parsedObject["value"].ToString();
                mitarbeiters = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Mitarbeiter>>(popupJson));

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\tBeregnungsDaten erfolgreich geladen.");
                }
            }
            return mitarbeiters;
        }

        /// <summary>
        /// Update Mitarbeiter
        /// </summary>
        /// <param name="daten">neue Daten</param>
        /// <returns> Statuscode</returns>
        public async Task<bool> UpdateDatenAsync(Mitarbeiter daten)
        {
            //Prüfen ob Daten null sind
            if (daten == null)
            {
                return false;
            }
            //In JSON konvertieren
            var serializedItem = JsonConvert.SerializeObject(daten);
            //An Server Senden
            var response = await _client.PutAsync($"api/betriebe/" + betriebID + "/mitarbeiters/" + daten.ID.ToString(), new StringContent(serializedItem, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
    }
}
