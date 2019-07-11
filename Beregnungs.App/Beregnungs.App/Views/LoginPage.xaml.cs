using Beregnungs.App.Models;
using Beregnungs.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Beregnungs.App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        public Account Account { get; set; }
        LoginViewModel viewModel;
        public Command LoginCommand;

        public LoginPage()
        {
            InitializeComponent();

            var account = new Account()
            {
                Benutzername = "Admin",
                Passwort = "nico123"
            };
            viewModel = new LoginViewModel(account);
            BindingContext = viewModel;            
        }
	}
}