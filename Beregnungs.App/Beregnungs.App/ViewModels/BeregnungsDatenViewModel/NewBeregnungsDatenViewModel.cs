using Beregnungs.App.Models;
using Beregnungs.App.Services;

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Beregnungs.App.ViewModels
{
    public class NewBeregnungsDatenViewModel : BaseViewModel
    {
        public IDataStore<BeregnungsDaten> DataStore => 
            DependencyService.Get<IDataStore<BeregnungsDaten>>() 
            ?? new BeregnungsDatenRESTStore();

        public ObservableCollection<BeregnungsDaten> BeregnungsDatens { get; set; }
        public BeregnungsDaten beregnungsDaten;
        public Command SaveNewBeregnungsDatensCommand { get; set; }

        DateTime startDatum = DateTime.Now;
        TimeSpan uhrzeit;
        DateTime endeDatum;
        string betrieb = "Bauer Heinrich";
        string schlag = "Feld 10";
        string duese = "Düsenmaster 3000";
        string wasseruhrStart = "35000";
        string wasseruhrEnde = "45000";
        string vorkomnisse = "Schlauch geplatzt";
        bool istAbgeschlossen = true;

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
        public string Betrieb
        {
            get { return betrieb; }
            set { betrieb = value; }
        }
        public string Schlag
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
            SaveNewBeregnungsDatensCommand = new Command(async () => await ExecuteSaveNewBeregnungsDatensCommand());
        }

        private async Task ExecuteSaveNewBeregnungsDatensCommand()
        {
            beregnungsDaten = new BeregnungsDaten()
            {
                StartDatum = DateTimeOffset.Parse(StartDatum.ToString()),
                StartUhrzeit = Convert.ToDateTime(Uhrzeit.ToString()),
                EndDatum = DateTimeOffset.Parse(EndDatum.ToString()),
                //SchlagID = new Guid(Schlag),
                WasseruhrAnfang = int.Parse(WasseruhrStart),
                WasseruhrEnde = int.Parse(WasseruhrEnde),
                Duese = Duese,
                Vorkomnisse = Vorkommnisse,
                IstAbgeschlossen = IstAbgeschlossen
        
            };
           await  DataStore.AddDatenAsync(beregnungsDaten);

            //Laden der neuen Daten
            await DataStore.GetDatensAsync(true);

        }
    }
}
