using System;
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


namespace Samples.Droid {

    [Activity(Label = "Samples", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity {

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            Forms.Init(this, bundle);
            UserDialogs.Init(this);
            this.LoadApplication(new App());
        }
    }
}

