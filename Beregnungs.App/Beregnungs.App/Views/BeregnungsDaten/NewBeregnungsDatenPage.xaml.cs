using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Beregnungs.App.Models;
using Beregnungs.App.ViewModels;

namespace Beregnungs.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewBeregnungsDatenPage : ContentPage
    {
        public BeregnungsDaten BeregnungsDaten { get; set; }
        public Command SaveNewBeregnungsDatensCommand;
        private NewBeregnungsDatenViewModel viewModel;

        public NewBeregnungsDatenPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new NewBeregnungsDatenViewModel();

        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.BeregnungsDatens.Count == 0)
                viewModel.LoadBeregnungsDatensCommand.Execute(null);
        }
        private void StackLayout_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}