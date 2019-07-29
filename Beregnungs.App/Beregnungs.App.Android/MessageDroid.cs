using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Beregnungs.App.Droid;
using Beregnungs.App.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MessageDroid))]
namespace Beregnungs.App.Droid
{

    public class MessageDroid : IMessage
    {
        //Langer Toast
        public void LongAlert(string message)
        {
            //Toast erstellen
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        //Kurzer Toast
        public void ShortAlert(string message)
        {
            //Toast erstellen
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}