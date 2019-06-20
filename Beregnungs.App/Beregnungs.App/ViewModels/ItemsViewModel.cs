using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Beregnungs.App.Models;
using Beregnungs.App.Views;

namespace Beregnungs.App.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        #region Fields
        //Fields
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        string start = string.Empty;
        string uhrzeit = string.Empty;
        string ende = string.Empty;
        string betrieb = string.Empty;
        string schlag = string.Empty;
        string duese = string.Empty;
        string wasseruhrStart = string.Empty;
        string wasseruhrEnde = string.Empty;
        string vorkomnisse = string.Empty;

        public string StartTitel
        {
            get { return start; }
            set { SetProperty(ref start, value); }
        }
        public string UhrzeitTitel
        {
            get { return uhrzeit; }
            set { SetProperty(ref uhrzeit, value); }
        }
        public string EndeTitel
        {
            get { return ende; }
            set { SetProperty(ref ende, value); }
        }
        public string BetriebTitel
        {
            get { return betrieb; }
            set { SetProperty(ref betrieb, value); }
        }
        public string SchlagTitel
        {
            get { return schlag; }
            set { SetProperty(ref schlag, value); }
        }
        public string DueseTitel
        {
            get { return duese; }
            set { SetProperty(ref duese, value); }
        }
        public string WasseruhrStartTitel
        {
            get { return wasseruhrStart; }
            set { SetProperty(ref wasseruhrEnde, value); }
        }
        public string WasseruhrEndeTitel
        {
            get { return wasseruhrEnde; }
            set { SetProperty(ref wasseruhrEnde, value); }
        }
        public string VorkomnisseTitel
        {
            get { return vorkomnisse; }
            set { SetProperty(ref vorkomnisse, value); }
        }
        #endregion

        #region Ctor
        // Ctor
        public ItemsViewModel()
        {
            Title = "Beregnungsdaten";
            StartTitel = "Tag der Beregnung";
            UhrzeitTitel = "Beginn der Beregnung";
            EndeTitel = "Ende der Beregnung";
            BetriebTitel = "Betrieb";
            SchlagTitel = "Schlag";
            DueseTitel = "Verwendete Düse";
            WasseruhrStartTitel = "Wasseruhrstand Start";
            WasseruhrEndeTitel = "Wasseruhrstand Ende";
            VorkomnisseTitel = "Vorkomnisse";

            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }
        #endregion

        #region Methods
        //Load Comand
        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
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