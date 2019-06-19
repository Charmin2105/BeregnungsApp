using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Beregnungs.App.Models;

namespace Beregnungs.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new Item()
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

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}