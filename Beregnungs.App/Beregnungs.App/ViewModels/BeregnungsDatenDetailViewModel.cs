using System;
using System.Threading.Tasks;
using Beregnungs.App.Models;
using Xamarin.Forms;

namespace Beregnungs.App.ViewModels
{
    public class BeregnungsDatenDetailViewModel : BaseViewModel
    {
        public BeregnungsDaten BeregnungsDaten { get; set; }
        public Command SaveBeregnungsDatensCommand { get; set; }
        public Command DeleteBeregnungsDatensCommand { get; set; }
        public string startDatum = string.Empty;

        public string StartDatum
        {
            get { return BeregnungsDaten.StartDatumString; }
            set { BeregnungsDaten.StartDatumString = value; }
        }
        public string StartUhrzeit
        {
            get { return BeregnungsDaten.StartUhrzeitString; }
            set { BeregnungsDaten.StartUhrzeitString = value; }
        }
        public string EndDatum
        {
            get { return BeregnungsDaten.EndDatumString; }
            set { BeregnungsDaten.EndDatumString = value; }
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
            await DataStore.DeleteAsync(BeregnungsDaten.ID);
        }

        private async Task ExecuteSaveBeregnungsDatensCommand()
        {
            await DataStore.UpdateAsync(BeregnungsDaten);
        }
    }
}
