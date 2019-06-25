using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Beregnungs.App.Models;
using Beregnungs.App.ViewModels;
using System.Threading.Tasks;

namespace Beregnungs.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public BeregnungsDaten BeregnungsDaten { get; set; }
        public Command SaveBeregnungsDatensCommand;
        private NewBeregnungsDatenViewModel viewModel;

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new NewBeregnungsDatenViewModel();

        }

        //async void Save_Clicked(object sender, EventArgs e)
        //{
        //    MessagingCenter.Send(this, "AddItem", BeregnungsDaten);
        //    await Navigation.PopModalAsync();
        //}

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void StackLayout_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}