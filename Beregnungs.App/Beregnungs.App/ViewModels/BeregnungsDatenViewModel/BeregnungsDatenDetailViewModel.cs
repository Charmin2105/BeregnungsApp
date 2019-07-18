using System;
using System.Threading.Tasks;
using Beregnungs.App.Models;
using Beregnungs.App.Services;
using Xamarin.Forms;

namespace Beregnungs.App.ViewModels
{
    public class BeregnungsDatenDetailViewModel : BaseViewModel
    {
        public IDataStore<BeregnungsDaten> DataStore => DependencyService.Get<IDataStore<BeregnungsDaten>>() ?? new BeregnungsDatenRESTStore();

        public BeregnungsDaten BeregnungsDaten { get; set; }
        public Command SaveBeregnungsDatensCommand { get; set; }
        public Command DeleteBeregnungsDatensCommand { get; set; }
        public string startDatum = string.Empty;

        public DateTime StartDatum
        {
            get { return BeregnungsDaten.StartDatum.Date; }
            set { BeregnungsDaten.StartDatum = value; }
        }
        public TimeSpan StartUhrzeit
        {
            get { return BeregnungsDaten.Uhrzeit; }
            set { BeregnungsDaten.Uhrzeit = value; }
        }
        public DateTime EndDatum
        {
            get { return BeregnungsDaten.EndDatum.Date; }
            set { BeregnungsDaten.EndDatum = value; }
        }
        public string SchlagID
        {
            get { return BeregnungsDaten.SchlagIDString; }
            set { BeregnungsDaten.SchlagIDString = value; }
        }
        public int WasseruhrAnfang
        {
            get { return BeregnungsDaten.WasseruhrAnfang; }
            set { BeregnungsDaten.WasseruhrAnfang = value; }
        }
        public int WasseruhrEnde
        {
            get { return BeregnungsDaten.WasseruhrEnde; }
            set { BeregnungsDaten.WasseruhrEnde = value; }
        }
        public string BetriebID
        {
            get { return BeregnungsDaten.BetriebIDString; }
            set { BeregnungsDaten.BetriebIDString = value; }
        }
        public string Duese
        {
            get { return BeregnungsDaten.Duese; }
            set { BeregnungsDaten.Duese = value; }
        }
        public string Vorkommnisse
        {
            get { return BeregnungsDaten.Vorkomnisse; }
            set { BeregnungsDaten.Vorkomnisse = value; }
        }
        public bool IstAbgeschlossen
        {
            get { return BeregnungsDaten.IstAbgeschlossen; }
            set { BeregnungsDaten.IstAbgeschlossen = value; }
        }

        public BeregnungsDatenDetailViewModel(BeregnungsDaten beregnungsDaten = null)
        {
            Title = "Beregnungsdaten";
            BeregnungsDaten = beregnungsDaten;
            SaveBeregnungsDatensCommand = new Command(async () => await ExecuteSaveBeregnungsDatensCommand());
            DeleteBeregnungsDatensCommand = new Command(async () => await ExecuteDeleteBeregnungsDatensCommand());
        }

        private async Task ExecuteDeleteBeregnungsDatensCommand()
        {
            await DataStore.DeleteDatenAsync(BeregnungsDaten.ID);
        }

        private async Task ExecuteSaveBeregnungsDatensCommand()
        {
            await DataStore.UpdateDatenAsync(BeregnungsDaten);
        }
    }
}
