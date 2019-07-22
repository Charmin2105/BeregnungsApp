using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Beregnungs.App.Models;
using Beregnungs.App.ViewModels;


namespace Beregnungs.App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewMitarbeiterPage : ContentPage
	{
        public Mitarbeiter Mitarbeiter { get; set; }
        public Command SaveNewMitarbeiterCommand;
        private NewMitarbeiterViewModel viewModel;

        public NewMitarbeiterPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new NewMitarbeiterViewModel();
		}
        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private void StackLayout_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}