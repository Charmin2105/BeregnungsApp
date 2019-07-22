using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Beregnungs.App.Models;
using Beregnungs.App.ViewModels;
namespace Beregnungs.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MitarbeiterDetailPage : ContentPage
    {
        public Mitarbeiter Mitarbeiter { get; set; }
        MitarbeiterDetailViewModel viewModel;

        public MitarbeiterDetailPage(MitarbeiterDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }
    }
}