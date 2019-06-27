using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using Beregnungs.App.Models;
using Beregnungs.App.Services;

namespace Beregnungs.App.ViewModels
{
    public class BetriebDetailViewModel : BaseViewModel
    {
        public IDataStore<Betrieb> DataStore => DependencyService.Get<IDataStore<Betrieb>>() ?? new BetriebRESTStore();

        public Betrieb Betrieb { get; set; }
        public Command SaveBetriebCommand { get; set; }
        public Command DeleteBetriebCommand { get; set; }

        public string Name
        {
            get { return Betrieb.Name; }
            set { Betrieb.Name = value; }
        }

        public BetriebDetailViewModel(Betrieb betrieb = null)
        {
            Title = "Betrieb ändern";
            Betrieb = betrieb;
            SaveBetriebCommand = new Command(async () => await ExecuteSaveBetriebCommand());
            DeleteBetriebCommand = new Command(async () => await ExecuteDeleteBetriebCommand());
        }

        private Task ExecuteDeleteBetriebCommand()
        {
            throw new NotImplementedException();
        }

        private Task ExecuteSaveBetriebCommand()
        {
            throw new NotImplementedException();
        }
    }
}
