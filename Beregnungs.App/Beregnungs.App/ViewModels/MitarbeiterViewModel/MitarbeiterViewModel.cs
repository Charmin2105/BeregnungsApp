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
    public class MitarbeiterViewModel : BaseViewModel
    {
        //fields
        private IDataStore<Mitarbeiter> DataStore => DependencyService.Get<IDataStore<Mitarbeiter>>() ?? new MitarbeiterRESTStore();
        public ObservableCollection<Mitarbeiter> Mitarbeiters { get; set; }
        public Command LoadMitarbeitersCommand { get; set; }
        private string vorname = string.Empty;
        private string nachname = string.Empty;
        private string geburtstag = string.Empty;

        //Binding Context überschriften
        public string VornameTitel
        {
            get
            {
                return vorname;
            }
            set
            {
                SetProperty(ref vorname, value);
            }
        }
        public string NachnameTitel
        {
            get
            {
                return nachname;
            }
            set
            {
                SetProperty(ref nachname, value);
            }
        }
        public string GeburtstagTitel
        {
            get
            {
                return geburtstag;
            }
            set
            {
                SetProperty(ref geburtstag, value);
            }
        }

        public MitarbeiterViewModel()
        {
            Title = "Mitarbeiter";
            VornameTitel = "Vorname";
            NachnameTitel = "Name";
            GeburtstagTitel = "Geburtsdatum";

            Mitarbeiters = new ObservableCollection<Mitarbeiter>();
            LoadMitarbeitersCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        private async Task ExecuteLoadItemsCommand()
        {
            //Prüfung ob Internet Verbindung vorhanden ist
            if (CrossConnectivity.Current.IsConnected)
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                try
                {
                    Mitarbeiters.Clear();
                    var items = await DataStore.GetDatensAsync(true);
                    foreach (var item in items)
                    {
                        Mitarbeiters.Add(item);
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
    }
}
