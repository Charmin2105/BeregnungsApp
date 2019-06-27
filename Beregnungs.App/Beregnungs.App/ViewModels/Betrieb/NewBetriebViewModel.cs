using Beregnungs.App.Models;
using Beregnungs.App.Services;

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Beregnungs.App.ViewModels
{
    public class NewBetriebViewModel : BaseViewModel
    {
        public IDataStore<Betrieb> DataStore => DependencyService.Get<IDataStore<Betrieb>>() ?? new BetriebRESTStore();
        public ObservableCollection<Betrieb> Betrieb { get; set; }
        private Betrieb betrieb;
        public Command SaveNewBetriebCommand { get; set; }
        string name = string.Empty;

        public string Name
        {
            get
            {
                return name;
            }
            set { name = value; }
        }


        public NewBetriebViewModel()
        {
            Betrieb = new ObservableCollection<Betrieb>();
            SaveNewBetriebCommand = new Command(async () => await ExecuteSaveNewBetriebCommand());
        }

        private async Task ExecuteSaveNewBetriebCommand()
        {
            betrieb = new Betrieb()
            {
                Name = Name
            };
            await DataStore.AddDatenAsync(betrieb);
        }
    }
}
