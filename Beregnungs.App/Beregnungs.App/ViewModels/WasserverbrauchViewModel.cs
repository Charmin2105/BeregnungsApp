using Beregnungs.App.Models;
using Beregnungs.App.Services;
using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace Beregnungs.App.ViewModels
{
    public class WasserverbrauchViewModel : BaseViewModel
    {
        public IDataStore<BeregnungsDaten> DataStore => DependencyService.Get<IDataStore<BeregnungsDaten>>() ?? new BeregnungsDatenRESTStore();

        public Command LoadBeregnungsDatensCommand { get; set; }
        public LineChart Chart { get; set; }
        public int Verbrauch { get; set; }
        public Entry[] entry;
        public Entry[] Entry
        {
            get { return entry; }
            set { value = entry; }
        }


        public WasserverbrauchViewModel()
        {
            LoadBeregnungsDatensCommand = new Command(async () => await ExecuteLoadItemsCommand());
            entry = new[]
              {
                 new Entry(3500)
                 {
                     Label = "25.07.2019",
                     ValueLabel = "3500",
                     Color = SKColor.Parse("#2c3e50")
                 },
                 new Entry(3862)
                 {
                     Label = "26.07.2019",
                     ValueLabel = "3862",
                     Color = SKColor.Parse("#77d065")
                 },
                 new Entry(2356)
                 {
                     Label = "26.07.2019",
                     ValueLabel = "2356",
                     Color = SKColor.Parse("#b455b6")
                 },
                 new Entry(4639)
                 {
                     Label = "28.07.2019",
                     ValueLabel = "4639",
                     Color = SKColor.Parse("#3498db")
            } };
            Chart = new LineChart()
            {
                Entries = Entry
            };

        }

        #region Methods
        //Load Comand
        async Task ExecuteLoadItemsCommand()
        {
            try
            {
                var items = await DataStore.GetDatensAsync(true);
                foreach (var item in items)
                {
                    entry = new[]
                    {
                        new Entry(item.Verbrauch)
                        {
                            Label = item.StartDatumString,
                            ValueLabel = item.StartDatumString
                        }
                   };
                }
               Chart = new LineChart()
                {
                    Entries = entry
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion
    }
}
