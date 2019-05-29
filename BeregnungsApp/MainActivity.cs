using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Content;

namespace BeregnungsApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button buttonLogin;
        EditText editTextEmail;
        EditText editTextPassword;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //EditText initalisieren
             editTextEmail = FindViewById<EditText>(Resource.Id.editTextMainMail);
            editTextPassword = FindViewById<EditText>(Resource.Id.editTextMainPassword);

            // Button initalisieren
            buttonLogin = FindViewById<Button>(Resource.Id.buttonMainLogin);
            buttonLogin.Click += OnButtonLoginClicked;
        }
        public void OnButtonLoginClicked(object sender, EventArgs e)
        {
            var mail = editTextEmail.Text.ToString();
            var password =  editTextPassword.Text.ToString();
            Login(mail, password);
            Intent intent = new Intent(this, typeof(InputActivity));
            StartActivity(intent);
        }
        private void Login(string mail, string password)
        {

        }
    }
}