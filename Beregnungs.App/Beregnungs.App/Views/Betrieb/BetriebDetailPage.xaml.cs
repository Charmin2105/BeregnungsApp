using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Beregnungs.App.Models;
using Beregnungs.App.ViewModels;

namespace Beregnungs.App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BetriebDetailPage : ContentPage
	{
        public Betrieb Betrieb { get; set; }
        BetriebDetailViewModel viewModel;
        public Command SaveBetriebCommand;

        //Ctor mit Parameter
        public BetriebDetailPage (BetriebDetailViewModel viewModel)
		{
			InitializeComponent ();
            BindingContext = this.viewModel = viewModel;
		}

        public BetriebDetailPage()
        {
            InitializeComponent();
            var betrieb = new Betrieb()
            {
                ID = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf"),
                Name = "SudiFarm"
            };
            viewModel = new BetriebDetailViewModel(betrieb);
            BindingContext = viewModel;

        }
	}
}