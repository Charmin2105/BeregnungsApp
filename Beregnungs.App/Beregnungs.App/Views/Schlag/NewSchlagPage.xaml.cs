using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Beregnungs.App.Models;
using Beregnungs.App.ViewModels;

namespace Beregnungs.App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewSchlagPage : ContentPage
	{
        public Schlag Schlag { get; set; }
        public Command SaveNewSchlagCommand;
        public NewSchlagViewModel viewModel;


        public NewSchlagPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new NewSchlagViewModel();
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