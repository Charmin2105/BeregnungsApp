using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using Beregnungs.App.Models;
using Beregnungs.App.Services;

namespace Beregnungs.App.ViewModels
{
    public class SchlagDetailViewModel : BaseViewModel
    {
        public IDataStore<Schlag> DataStore => DependencyService.Get<IDataStore<Schlag>>() ?? new SchlagRESTStore();

        public Schlag Schlag { get; set; }
        public Command SaveSchlagCommand { get; set; }
        public Command DeleteSchlagCommand { get; set; }

        public string Name
        {
            get { return Schlag.Name; }
            set { Schlag.Name = value; }
        }
        public string BetriebID
        {
            get { return Schlag.BetriebID.ToString(); }
            set { Schlag.BetriebID = Guid.Parse( value); }
        }

        public SchlagDetailViewModel(Schlag schlag = null)
        {
            Title = "Schlag ändern";
            Schlag = schlag;
            SaveSchlagCommand = new Command(async () => await ExecuteSaveSchlagCommand());
            DeleteSchlagCommand = new Command(async () => await ExecuteDeleteSchlagCommand());

        }

        private async Task ExecuteDeleteSchlagCommand()
        {
           await  DataStore.DeleteDatenAsync(Schlag.ID);
        }

        private async Task ExecuteSaveSchlagCommand()
        {
            await DataStore.UpdateDatenAsync(Schlag);
        }
    }
}
