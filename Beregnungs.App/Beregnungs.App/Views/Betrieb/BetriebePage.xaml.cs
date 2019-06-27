using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Beregnungs.App.Models;
using Beregnungs.App.ViewModels;

namespace Beregnungs.App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BetriebePage : ContentPage
	{
        BetriebViewModel viewModel;

        //ctor
		public BetriebePage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new BetriebViewModel();
		}

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewBetriebPage()));
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Betrieb;
            if (item == null)
                return;

            await Navigation.PushAsync(new BetriebDetailPage(new BetriebDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Betrieb.Count == 0)
                viewModel.LoadBetriebCommand.Execute(null);
        }
    }
}