using Beregnungs.App.Models;
using Beregnungs.App.Services;
using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace Beregnungs.App.ViewModels
{
    public class WasserverbrauchViewModel : BaseViewModel
    {
        public IDataStore<BeregnungsDaten> DataStore => DependencyService.Get<IDataStore<BeregnungsDaten>>() ?? new BeregnungsDatenRESTStore();

        public ObservableCollection<BeregnungsDaten> BeregnungsDatens { get; set; }
        public Command LoadBeregnungsDatensCommand { get; set; }
        public LineChart Chart
        {
            get { return Chart; }
            set { value = Chart; }
        }
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
            BeregnungsDatens = new ObservableCollection<BeregnungsDaten>();
            entry = new[]
              {
                 new Entry(25)
                 {
                     Label = "UWP",
                     ValueLabel = "212",
                     Color = SKColor.Parse("#2c3e50")
                 },
                 new Entry(248)
                 {
                     Label = "Android",
                     ValueLabel = "248",
                     Color = SKColor.Parse("#77d065")
                 },
                 new Entry(128)
                 {
                     Label = "iOS",
                     ValueLabel = "128",
                     Color = SKColor.Parse("#b455b6")
                 },
                 new Entry(514)
                 {
                     Label = "Shared",
                     ValueLabel = "514",
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
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                BeregnungsDatens.Clear();
                var items = await DataStore.GetDatensAsync(true);
                foreach (var item in items)
                {
                    BeregnungsDatens.Add(item);
                    entry = new[]
                    {
                        new Entry(item.Verbrauch)
                        {
                            Label = item.StartDatumString,
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
