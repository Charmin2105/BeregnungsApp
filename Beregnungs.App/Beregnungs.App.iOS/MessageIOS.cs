using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beregnungs.App.iOS;
using Beregnungs.App.Services;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(MessageIOS))]
namespace Beregnungs.App.iOS
{
    public class MessageIOS : IMessage
    {
        //Fields
        const double langerToast = 3.5;
        const double kurzerToast = 2.0;

        NSTimer delay;
        UIAlertController messageDialog;

        //Lange Message Anzeigen
        public void LongAlert(string message)
        {
            ShowIOSToast(message, langerToast);
        }
        //Kurze Message anzeigen
        public void ShortAlert(string message)
        {
            ShowIOSToast(message, kurzerToast);
        }

        //Message Dialog erstellen
        void ShowIOSToast(string message, double seconds)
        {
            delay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                MessageBeenden();
            });
            messageDialog = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(messageDialog, true, null);
        }

        //Message Dialog beenden
        void MessageBeenden()
        {
            if (messageDialog != null)
            {
                messageDialog.DismissViewController(true, null);
            }
            if (delay != null)
            {
                delay.Dispose();
            }
        }
    }
}