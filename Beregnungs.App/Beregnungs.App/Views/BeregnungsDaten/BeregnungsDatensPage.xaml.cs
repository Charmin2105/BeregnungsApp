using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Beregnungs.App.Models;
using Beregnungs.App.ViewModels;

namespace Beregnungs.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BeregnungsDatensPage : ContentPage
    {
        BeregnungsDatensViewModel viewModel;

        public BeregnungsDatensPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new BeregnungsDatensViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as BeregnungsDaten;
            if (item == null)
                return;

            await Navigation.PushAsync(new BeregnungsDatenDetailPage(new BeregnungsDatenDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewBeregnungsDatenPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.BeregnungsDatens.Count == 0)
                viewModel.LoadBeregnungsDatensCommand.Execute(null);
        }

    }
}