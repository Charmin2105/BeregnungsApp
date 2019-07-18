using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Beregnungs.App.Models;
using Beregnungs.App.ViewModels;

namespace Beregnungs.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BeregnungsDatenDetailPage : ContentPage
    {
        public BeregnungsDaten BeregnungsDaten { get; set; }
        BeregnungsDatenDetailViewModel viewModel;
        public Command SaveBeregnungsDatensCommand;

        public BeregnungsDatenDetailPage(BeregnungsDatenDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }


    }
}