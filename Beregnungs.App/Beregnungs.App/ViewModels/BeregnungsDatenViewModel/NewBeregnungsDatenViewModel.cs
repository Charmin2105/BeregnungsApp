using Beregnungs.App.Models;
using Beregnungs.App.Services;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Beregnungs.App.ViewModels
{
    public class NewBeregnungsDatenViewModel : BaseViewModel
    {
        public IDataStore<BeregnungsDaten> DataStore => 
            DependencyService.Get<IDataStore<BeregnungsDaten>>() 
            ?? new BeregnungsDatenRESTStore();

        public IDataStore<Schlag> SchlagDataStore => DependencyService.Get<IDataStore<Schlag>>() ?? new SchlagRESTStore();

        public ObservableCollection<BeregnungsDaten> BeregnungsDatens { get; set; }
        public ObservableCollection<Schlag> Schlags { get; set; }
        private BeregnungsDaten beregnungsDaten;
        public Command SaveNewBeregnungsDatensCommand { get; set; }
        public Command LoadBeregnungsDatensCommand { get; set; }

        DateTime startDatum = DateTime.Now;
        TimeSpan uhrzeit = DateTime.Now.TimeOfDay;
        DateTime endeDatum = DateTime.Now;
        Betrieb betrieb;
        Schlag schlag;
        string duese = string.Empty;
        string wasseruhrStart = string.Empty;
        string wasseruhrEnde = string.Empty;
        string vorkomnisse = string.Empty;
        bool istAbgeschlossen = false;

        public DateTime StartDatum
        {
            get { return startDatum; }
            set { startDatum = value; }
        }
        public TimeSpan Uhrzeit
        {
            get { return uhrzeit; }
            set { uhrzeit = value; }
        }
        public DateTime EndDatum
        {
            get { return endeDatum; }
            set { endeDatum = value; }
        }
        public Betrieb Betrieb
        {
            get { return betrieb; }
            set { betrieb = value; }
        }
        public Schlag SelectedSchlag
        {
            get { return schlag; }
            set { schlag = value; }
        }
        public string Duese
        {
            get { return duese; }
            set { duese = value; }
        }
        public string WasseruhrStart
        {
            get { return wasseruhrStart; }
            set { wasseruhrStart = value; }
        }
        public string WasseruhrEnde
        {
            get { return wasseruhrEnde; }
            set { wasseruhrEnde = value; }
        }
        public string Vorkommnisse
        {
            get { return vorkomnisse; }
            set { vorkomnisse = value; }
        }
        public bool IstAbgeschlossen
        {
            get { return istAbgeschlossen; }
            set { istAbgeschlossen = value; }
        }


        public NewBeregnungsDatenViewModel()
        {
            BeregnungsDatens = new ObservableCollection<BeregnungsDaten>();
            Schlags = new ObservableCollection<Schlag>();
            LoadBeregnungsDatensCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SaveNewBeregnungsDatensCommand = new Command(async () => await ExecuteSaveNewBeregnungsDatensCommand());
        }

        private async Task ExecuteLoadItemsCommand()
        {
            try
            {
                Schlags.Clear();
                var items = await SchlagDataStore.GetDatensAsync(true);
                foreach (var item in items)
                {
                    Schlags.Add(item);                    
                }                     
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task ExecuteSaveNewBeregnungsDatensCommand()
        {
            beregnungsDaten = new BeregnungsDaten()
            {
                StartDatum = DateTimeOffset.Parse(StartDatum.ToString()),
                StartUhrzeit = Convert.ToDateTime(Uhrzeit.ToString()),
                EndDatum = DateTimeOffset.Parse(EndDatum.ToString()),
                SchlagID = schlag.ID,
                WasseruhrAnfang = int.Parse(WasseruhrStart),
                WasseruhrEnde = int.Parse(WasseruhrEnde),
                Duese = Duese,
                Vorkomnisse = Vorkommnisse,
                IstAbgeschlossen = IstAbgeschlossen
        
            };
           await  DataStore.AddDatenAsync(beregnungsDaten);
            DependencyService.Get<IMessage>().LongAlert("Beregnungsdaten erfolgreich gespeichert");

        }
    }
}
