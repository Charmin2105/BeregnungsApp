using Beregnungs.App.Models;
using Beregnungs.App.Services;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Beregnungs.App.ViewModels
{
    public class NewMitarbeiterViewModel : BaseViewModel
    {
        //fields
        private IDataStore<Mitarbeiter> DataStore => DependencyService.Get<IDataStore<Mitarbeiter>>() ?? new MitarbeiterRESTStore();

        public ObservableCollection<Mitarbeiter> Mitarbeiters { get; set; }

        public Command SaveNewMitarbeiterCommand { get; set; }
        private Mitarbeiter mitarbeiter;
        private string vorname = string.Empty;
        private string nachname = string.Empty;
        private DateTime gebDatum;

        //DataBinding Accessor
        public string Vorname
        {
            get { return vorname; }
            set { vorname = value; }
        }
        public string Nachname
        {
            get { return nachname; }
            set { nachname = value; }
        }
        public DateTime GebDatum
        {
            get { return gebDatum; }
            set { gebDatum = value; }
        }

        //ctor
        public NewMitarbeiterViewModel()
        {
            Mitarbeiters = new ObservableCollection<Mitarbeiter>();
            SaveNewMitarbeiterCommand = new Command(async () => await ExecuteSaveNewMitarbeiterCommand());
        }

        private async Task ExecuteSaveNewMitarbeiterCommand()
        {
            mitarbeiter = new Mitarbeiter()
            {
                Vorname = Vorname,
                Nachname = Nachname,
                Geburtstag = Convert.ToDateTime( GebDatum)
            };

            await DataStore.AddDatenAsync(mitarbeiter);
        }
    }
}
