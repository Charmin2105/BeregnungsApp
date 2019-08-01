using Beregnungs.App.Models;
using Beregnungs.App.ViewModels;
using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;

namespace Beregnungs.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WasserverbrauchPage : ContentPage
    {
        public BeregnungsDaten BeregnungsDaten { get; set; }
        WasserverbrauchViewModel viewModel;

        public WasserverbrauchPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new WasserverbrauchViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadBeregnungsDatensCommand.Execute(null);
        }
    }
}