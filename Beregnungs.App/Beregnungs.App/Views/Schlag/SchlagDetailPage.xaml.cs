using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Beregnungs.App.Models;
using Beregnungs.App.ViewModels;

namespace Beregnungs.App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SchlagDetailPage : ContentPage
	{
        public Schlag Schlag { get; set; }
        SchlagDetailViewModel viewModel;
        public Command SaveSchlagCommand;

        public SchlagDetailPage ( SchlagDetailViewModel viewModel)
		{
			InitializeComponent ();
            BindingContext = this.viewModel = viewModel;
		}

        public SchlagDetailPage()
        {
            InitializeComponent();

            var schlag = new Schlag()
            {
                  ID = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf"),
                  Name = "Feld 1",
            };
            viewModel = new SchlagDetailViewModel(schlag);
            BindingContext = viewModel;
        }
	}
}