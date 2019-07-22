using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Beregnungs.App.Models;
using Beregnungs.App.ViewModels;

namespace Beregnungs.App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MitarbeiterPage : ContentPage
	{
        MitarbeiterViewModel viewModel;

		public MitarbeiterPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new MitarbeiterViewModel();
		}

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Mitarbeiter;
            if (item == null)
                return;

            await Navigation.PushAsync(new MitarbeiterDetailPage(new MitarbeiterDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }
        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewMitarbeiterPage()));
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Mitarbeiters.Count == 0)
                viewModel.LoadMitarbeitersCommand.Execute(null);
        }
    }
}