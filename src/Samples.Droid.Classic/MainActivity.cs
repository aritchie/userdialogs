using System;
using Acr.UserDialogs;
using Android.App;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


namespace Samples.Droid.Classic
{
    [Activity(Label = "Samples.Droid.Classic", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Forms.Init(this, bundle);

            UserDialogs.Init(this);
            this.LoadApplication(new App());
        }
    }
}

