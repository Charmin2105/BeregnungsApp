using Beregnungs.App.Models;
using Beregnungs.App.Services;

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Beregnungs.App.ViewModels
{
    public class NewSchlagViewModel : BaseViewModel
    {
        public IDataStore<Schlag> DataStore => DependencyService.Get<IDataStore<Schlag>>() ?? new SchlagRESTStore();

        public ObservableCollection<Schlag> Schlag { get; set; }
        private Schlag schlag;
        public Command SaveNewSchlagCommand { get; set; }

        string name = string.Empty;

        public string Name
        {
            get
            {
                return name;
            }
            set { name = value; }
        }

        //Ctor
        public NewSchlagViewModel()
        {
            Schlag = new ObservableCollection<Schlag>();
            SaveNewSchlagCommand = new Command(async () => await ExecuteSaveNewSchlagCommand());
        }

        private async Task ExecuteSaveNewSchlagCommand()
        {
            schlag = new Schlag()
            {
                Name = Name
            };
            await DataStore.AddDatenAsync(schlag);
        }
    }
}
