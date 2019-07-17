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
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        public static string AzureBackendUrl = "http://localhost:5000";
        public static bool UseMockDataStore = false;


        //Token
        public static string Token;
        //BetirebID
        public static string BetriebID = "25320c5e-f58a-4b1f-b63a-8ee07a840bdf";

        //Authentication
        public static bool IsLogedIn = false;
        public static int LogInTimer = 60;

        //API Url
        public static string RESTBackendURL = "http://192.168.0.111:51872";

        public App()
        {
            InitializeComponent();

            if (UseMockDataStore)
                DependencyService.Register<MockDataStore>();
            else
            {
                DependencyService.Register<BeregnungsDatenRESTStore>();
                DependencyService.Register<SchlagRESTStore>();
            }
            //    DependencyService.Register<AzureDataStore>();

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
