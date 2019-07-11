using Beregnungs.App.Models;
using Beregnungs.App.Services;
using Beregnungs.App.Views;
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

        public Account Account { get; set; }
        public Command LoginCommand { get; set; }

        private bool areCredentialsInvalid = false;
        public bool AreCredentialsInvalid
        {
            get { return areCredentialsInvalid; }
            set { SetProperty(ref areCredentialsInvalid, value); }
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
            LoginCommand = new Command(async () => await ExecuteLoginCommand());
        }

        private async Task ExecuteLoginCommand()
        {

            await Authenticate.Login(Account);

            App.IsLogedIn = true;
            Application.Current.MainPage = new MainPage();
        }
    }
}
