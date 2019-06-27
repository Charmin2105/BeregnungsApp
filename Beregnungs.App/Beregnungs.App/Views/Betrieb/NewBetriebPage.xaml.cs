using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Beregnungs.App.Models;
using Beregnungs.App.ViewModels;

namespace Beregnungs.App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewBetriebPage : ContentPage
	{
        public Betrieb Betrieb { get; set; }
        public Command SaveNewBetriebCommand;
        public NewBetriebViewModel viewModel;

        public NewBetriebPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new NewBetriebViewModel();
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