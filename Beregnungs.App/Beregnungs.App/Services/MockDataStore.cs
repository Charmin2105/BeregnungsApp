using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beregnungs.App.Models;

namespace Beregnungs.App.Services
{
    public class MockDataStore //: IDataStore<BeregnungsDaten>
    {
        List<BeregnungsDaten> items;

        public MockDataStore()
        {
            items = new List<BeregnungsDaten>();
            var mockItems = new List<BeregnungsDaten>
            {
                    new BeregnungsDaten()
                    {
                        ID = new Guid(),
                        StartDatum = new DateTimeOffset(new DateTime(2019, 5, 21)),
                        StartUhrzeit = DateTime.Today,
                        EndDatum = new DateTimeOffset(new DateTime(2019, 5, 23)),
                        BetriebID = new Guid("51004c54-3a86-4f55-b1a7-c6caeb8ca522"),
                        SchlagID = new Guid("51004c54-3a86-4f55-b1a7-c6caeb8ca532"),
                        Duese = "Düsenmaster 3000",
                        WasseruhrAnfang=0,
                        WasseruhrEnde=2000,
                        Vorkomnisse = "Keine",
                        IstAbgeschlossen= true
                    },

            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(BeregnungsDaten item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(BeregnungsDaten item)
        {
            var oldItem = items.Where((BeregnungsDaten arg) => arg.ID == item.ID).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Guid id)
        {
            var oldItem = items.Where((BeregnungsDaten arg) => arg.ID == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<BeregnungsDaten> GetItemAsync(Guid id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.ID == id));
        }

        public async Task<IEnumerable<BeregnungsDaten>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}