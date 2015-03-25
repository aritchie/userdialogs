using System;
using Acr.UserDialogs;
using Android.App;
using Android.OS;


namespace OldAndroid {

    [Activity(Label = "OldAndroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity {

        protected override void OnCreate(Bundle bundle) {
            UserDialogs.Init(() => this);
            base.OnCreate(bundle);
            this.SetContentView(Resource.Layout.Main);
        }
    }
}

