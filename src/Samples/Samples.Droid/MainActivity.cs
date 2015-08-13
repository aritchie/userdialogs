using System;
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using NativeCode.Mobile.AppCompat.FormsAppCompat;
using Xamarin.Forms;


namespace Samples.Droid {

    [Activity(Label = "Samples", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, Theme = CompatThemeLightDarkActionBar)]
    public class MainActivity : AppCompatFormsApplicationActivity {

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            Forms.Init(this, bundle);
            UserDialogs.Init(this);
            this.LoadApplication(new App());
        }
    }
}

