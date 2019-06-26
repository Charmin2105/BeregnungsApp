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
        public IDataStore<BeregnungsDaten> DataStore => DependencyService.Get<IDataStore<BeregnungsDaten>>() ?? new BeregnungsDatenRESTStore();

        public ObservableCollection<BeregnungsDaten> BeregnungsDatens { get; set; }
        public BeregnungsDaten beregnungsDaten;
        public Command SaveNewBeregnungsDatensCommand { get; set; }

        string startDatum = "08.07.2016";
        string uhrzeit = "13:30:35";
        string endeDatum = "18.07.2016";
        string betrieb = "Bauer Heinrich";
        string schlag = "Feld 10";
        string duese = "Düsenmaster 3000";
        string wasseruhrStart = "35000";
        string wasseruhrEnde = "45000";
        string vorkomnisse = "Schlauch geplatzt";
        bool istAbgeschlossen = true;

        public string StartDatum
        {
            get { return startDatum; }
            set { startDatum = value; }
        }
        public string Uhrzeit
        {
            get { return uhrzeit; }
            set { uhrzeit = value; }
        }
        public string EndDatum
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
                StartDatum = DateTimeOffset.Parse(StartDatum),
                StartUhrzeit = DateTime.Parse(Uhrzeit),
                EndDatum = DateTimeOffset.Parse(EndDatum),
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
