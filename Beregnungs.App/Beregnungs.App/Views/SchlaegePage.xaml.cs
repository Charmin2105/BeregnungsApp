using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Beregnungs.App.Models;
using Beregnungs.App.Views;
using Beregnungs.App.ViewModels;

namespace Beregnungs.App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SchlaegePage : ContentPage
	{
        SchlageViewModel viewModel;

		public SchlaegePage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new SchlageViewModel();
		}
        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewSchlagPage()));
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Schlag;
            if (item == null)
                return;

            await Navigation.PushAsync(new SchlagDetailPage(new SchlagDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Schlag.Count == 0)
                viewModel.LoadSchlaegeCommand.Execute(null);
        }
    }
}