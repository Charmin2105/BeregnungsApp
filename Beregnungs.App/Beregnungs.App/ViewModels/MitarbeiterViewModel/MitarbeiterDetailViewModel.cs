using System;
using System.Threading.Tasks;
using Beregnungs.App.Models;
using Beregnungs.App.Services;
using Xamarin.Forms;

namespace Beregnungs.App.ViewModels
{
    public class MitarbeiterDetailViewModel : BaseViewModel
    {
        //Fields
        public IDataStore<Mitarbeiter> DataStore => DependencyService.Get<IDataStore<Mitarbeiter>>() ?? new MitarbeiterRESTStore();
        public Command SaveMitarbeiterCommand { get; set; }
        public Command DeleteMitarbeiterCommand { get; set; }
        public Mitarbeiter Mitarbeiter { get; set; }

        //DataBinding Accessor
        public string Vorname
        {
            get { return Mitarbeiter.Vorname; }
            set { Mitarbeiter.Vorname = value; }
        }
        public string Nachname
        {
            get { return Mitarbeiter.Nachname; }
            set { Mitarbeiter.Nachname = value; }
        }
        public DateTime GebDatum
        {
            get { return Mitarbeiter.Geburtstag.Date; }
            set { Mitarbeiter.Geburtstag = value; }
        }
        public string BetriebID
        {
            get { return Mitarbeiter.BetriebID.ToString(); }

        }

        public MitarbeiterDetailViewModel(Mitarbeiter mitarbeiter = null)
        {
            Title = "Mitarbeiter";
            Mitarbeiter = mitarbeiter;
            SaveMitarbeiterCommand = new Command(async () => await ExecuteSaveMitarbeiterCommand());
            DeleteMitarbeiterCommand = new Command(async () => await ExecuteDeleteMitarbeiterCommand());
        }

        private async Task ExecuteDeleteMitarbeiterCommand()
        {
            await DataStore.DeleteDatenAsync(Mitarbeiter.ID);
            DependencyService.Get<IMessage>().LongAlert("Löschen erfolgreich");
            //Beenden der Page (nicht funktionsfähig immoment)
            // await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async Task ExecuteSaveMitarbeiterCommand()
        {
            await DataStore.UpdateDatenAsync(Mitarbeiter);
            DependencyService.Get<IMessage>().LongAlert("Speichern erfolgreich");
        }
    }
}
