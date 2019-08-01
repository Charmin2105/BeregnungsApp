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

        //Ctor mit Parameter
        public BetriebDetailPage (BetriebDetailViewModel viewModel)
		{
			InitializeComponent ();
            BindingContext = this.viewModel = viewModel;
		}

        public BetriebDetailPage()
        {
            InitializeComponent();

            viewModel = new BetriebDetailViewModel(Betrieb);
            BindingContext = viewModel;

        }
	}
}