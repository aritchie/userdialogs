using System;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Cirrious.CrossCore.Plugins;
using Acr.UserDialogs;


namespace Acr.MvvmCross.Plugins.UserDialogs.Droid {

    public class Plugin : IMvxPlugin {

        public void Load() {
            Acr.UserDialogs.UserDialogs.Instance = new UserDialogsImpl(() => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity, true);
            Mvx.RegisterSingleton(Acr.UserDialogs.UserDialogs.Instance);
        }
    }
}