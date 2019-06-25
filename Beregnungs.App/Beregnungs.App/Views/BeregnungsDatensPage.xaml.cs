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

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.BeregnungsDatens.Count == 0)
                viewModel.LoadBeregnungsDatensCommand.Execute(null);
        }
    }
}