using Beregnungs.App.Models;
using Beregnungs.App.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Beregnungs.App.ViewModels
{
    public class BetriebViewModel : BaseViewModel
    {
        public IDataStore<Betrieb> DataStore => DependencyService.Get<IDataStore<Betrieb>>() ?? new BetriebRESTStore();
        public ObservableCollection<Betrieb> Betrieb { get; set; }
        public Command LoadBetriebCommand { get; set; }


        public BetriebViewModel()
        {
            Title = "Betriebe";
            Betrieb = new ObservableCollection<Betrieb>();
            LoadBetriebCommand = new Command(async () => await ExecuteLoadBetriebCommand());
        }

        private async Task ExecuteLoadBetriebCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Betrieb.Clear();
                var items = await DataStore.GetDatensAsync(true);
                foreach (var item in items)
                {
                    Betrieb.Add(item);
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
