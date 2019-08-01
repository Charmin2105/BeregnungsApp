using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Beregnungs.App.Services;
using Beregnungs.App.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Beregnungs.App
{
    public partial class App : Application
    {
        #region Fields
        //Fields

        //TODO  für die Zukunft über REST API immer Akutellen Wert anfordern
        public static double Wasserpreis = 0.15;

        //Token
        public static string Token;
        //BetirebID
        public static string BetriebID;

        //Authentication
        public static bool IsLogedIn = false;

        //TODO Für die Zukunft Rückmeldung an den Benutzer das der Token nicht mehr Gültig ist.
        public static int LogInTimer = 60;

        //API Url
        public static string RESTBackendURL = "http://192.168.0.111:51872"; 
        #endregion

        public App()
        {
            InitializeComponent();

            if (!IsLogedIn)
                MainPage = new NavigationPage(new LoginPage());
            else
                MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps

        }

        protected override void OnResume()
        {
            if (!IsLogedIn)
                MainPage = new NavigationPage(new LoginPage());
            else
                MainPage = new MainPage();
        }
    }
}
