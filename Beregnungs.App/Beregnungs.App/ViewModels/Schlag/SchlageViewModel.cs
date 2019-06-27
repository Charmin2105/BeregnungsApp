using Beregnungs.App.Models;
using Beregnungs.App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Beregnungs.App.ViewModels
{
    public class SchlageViewModel : BaseViewModel
    {
        public IDataStore<Schlag> DataStore => DependencyService.Get<IDataStore<Schlag>>() ?? new SchlagRESTStore();

        public ObservableCollection<Schlag> Schlag { get; set; }
        public Command LoadSchlaegeCommand { get; set; }
        public string name = string.Empty;

        public string NameTitel
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public SchlageViewModel()
        {
            Title = "Schläge";
            NameTitel = "Name des Schlags";
            Schlag = new ObservableCollection<Schlag>();
            LoadSchlaegeCommand = new Command(async () => await ExecuteLoadSchlagCommand());
        }

        private async Task ExecuteLoadSchlagCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Schlag.Clear();
                var items = await DataStore.GetDatensAsync(true);
                foreach (var item in items)
                {
                    Schlag.Add(item);
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
    }
}
