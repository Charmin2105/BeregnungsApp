using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Beregnungs.App.Models;
using Beregnungs.App.Views;
using Beregnungs.App.Services;
using Plugin.Connectivity;

namespace Beregnungs.App.ViewModels
{
    public class BeregnungsDatensViewModel : BaseViewModel
    {
        #region Fields
        //Fields
        public IDataStore<BeregnungsDaten> DataStore => DependencyService.Get<IDataStore<BeregnungsDaten>>() ?? new BeregnungsDatenRESTStore();

        public ObservableCollection<BeregnungsDaten> BeregnungsDatens { get; set; }
        public Command LoadBeregnungsDatensCommand { get; set; }
        string start = string.Empty;
        string uhrzeit = string.Empty;
        string ende = string.Empty;
        string betrieb = string.Empty;
        string schlag = string.Empty;
        string duese = string.Empty;
        string verbrauchTitel = string.Empty;
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
        public string VerbrauchTitel
        {
            get { return verbrauchTitel; }
            set { SetProperty(ref verbrauchTitel, value); }
        }
        public string VorkomnisseTitel
        {
            get { return vorkomnisse; }
            set { SetProperty(ref vorkomnisse, value); }
        }

        #endregion

        #region Ctor
        // Ctor
        public BeregnungsDatensViewModel()
        {
            Title = "Beregnungsdaten";
            StartTitel = "Tag der Beregnung";
            UhrzeitTitel = "Beginn der Beregnung";
            EndeTitel = "Ende der Beregnung";
            BetriebTitel = "Betrieb";
            SchlagTitel = "Schlag";
            DueseTitel = "Verwendete Düse";
            VerbrauchTitel = "Verbrauchtes Wasser in m³";
            VorkomnisseTitel = "Vorkomnisse";

            BeregnungsDatens = new ObservableCollection<BeregnungsDaten>();
            LoadBeregnungsDatensCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }
        #endregion

        #region Methods

        //Load Comand
        async Task ExecuteLoadItemsCommand()
        {
            //Prüfung ob Internet Verbindung vorhanden ist
            if (CrossConnectivity.Current.IsConnected)
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
            else
            {
                DependencyService.Get<IMessage>().LongAlert("Laden nicht möglich, kein Verbindung zum Server verfügbar");
                IsBusy = false;

            }
        } 
        #endregion
    }
}