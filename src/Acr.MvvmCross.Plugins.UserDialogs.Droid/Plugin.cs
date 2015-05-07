using System;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Cirrious.CrossCore.Plugins;


namespace Acr.MvvmCross.Plugins.UserDialogs.Droid {

    public class Plugin : IMvxPlugin {

        public void Load() {
            Acr.UserDialogs.UserDialogs.Init(() => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity);
            Mvx.RegisterSingleton(Acr.UserDialogs.UserDialogs.Instance);
        }
    }
}