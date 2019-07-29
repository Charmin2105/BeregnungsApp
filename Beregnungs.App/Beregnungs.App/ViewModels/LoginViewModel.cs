using Beregnungs.App.Models;
using Beregnungs.App.Services;
using Beregnungs.App.Views;
using Plugin.Connectivity;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Beregnungs.App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public IAuthenticate Authenticate => DependencyService.Get<IAuthenticate>() ?? new AuthenicateToApi();

        private string username;
        private string password;
        private string message;

        public Account Account { get; set; }
        public Command LoginCommand { get; set; }

        private bool areCredentialsInvalid = false;
        public bool AreCredentialsInvalid
        {
            get { return areCredentialsInvalid; }
            set { SetProperty(ref areCredentialsInvalid, value); }
        }
        public string Fehlermeldung
        {
            get { return message; }
            set { message = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }


        public LoginViewModel(Account account = null)
        {
            Account = account;
            if (CrossConnectivity.Current.IsConnected)
            {
                LoginCommand = new Command(async () => await ExecuteLoginCommand());
            }
            else
            {
                AreCredentialsInvalid = true;
                Fehlermeldung = "Keine Internetverbindung vorhanden. Anmelden nicht möglich";
            }
        }

        private async Task ExecuteLoginCommand()
        {
            if (Username == string.Empty || Password == string.Empty || Username == null || Password == null)
            {
                DependencyService.Get<IMessage>().LongAlert("Bitte Benutzernamen und Passwort eingeben");
                return;
            }

            Account = new Account()
            {
                Benutzername = Username,
                Passwort = Password
            };

            if (await Authenticate.Login(Account))
            {
                DependencyService.Get<IMessage>().LongAlert("Login erfolgreich");

                App.IsLogedIn = true;
                Application.Current.MainPage = new MainPage();
            }
            else
            {
                DependencyService.Get<IMessage>().LongAlert("Login Fehlgeschlagen");
                return;
            }
        }
    }
}
