using System;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Cirrious.CrossCore.Plugins;
using Acr.UserDialogs;


namespace Acr.MvvmCross.Plugins.UserDialogs.Droid {

    public class Plugin : IMvxPlugin {

        public void Load() {
            Mvx.CallbackWhenRegistered<IMvxAndroidCurrentTopActivity>(x => {
                Acr.UserDialogs.UserDialogs.Instance = new AppCompatUserDialogsImpl(() => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity);
                Mvx.RegisterSingleton(Acr.UserDialogs.UserDialogs.Instance);
            });
        }
    }
}