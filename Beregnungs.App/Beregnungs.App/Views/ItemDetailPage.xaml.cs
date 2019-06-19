using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Beregnungs.App.Models;
using Beregnungs.App.ViewModels;

namespace Beregnungs.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var item = new Item()
            {
                ID = new Guid(),
                StartDatum = new DateTimeOffset(new DateTime(2019, 5, 21)),
                StartUhrzeit = DateTime.Today,
                EndDatum = new DateTimeOffset(new DateTime(2019, 5, 23)),
                BetriebID = new Guid("51004c54-3a86-4f55-b1a7-c6caeb8ca522"),
                SchlagID = new Guid("51004c54-3a86-4f55-b1a7-c6caeb8ca532"),
                Duese = "Düsenmaster 3000",
                WasseruhrAnfang = 0,
                WasseruhrEnde = 2000,
                Vorkomnisse = "Keine",
                IstAbgeschlossen = true
            };

            viewModel = new ItemDetailViewModel(item);
            BindingContext = viewModel;
        }
    }
}