using System;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;


namespace Acr.MvvmCross.Plugins.UserDialogs.Touch {

    public class Plugin : IMvxPlugin {

        public void Load() {
            Acr.UserDialogs.UserDialogs.Init();
            Mvx.RegisterSingleton(Acr.UserDialogs.UserDialogs.Instance);
        }
    }
}