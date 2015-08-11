using System;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;


namespace Acr.MvvmCross.Plugins.UserDialogs.WP81 {

    public class Plugin : IMvxPlugin {

        public void Load() {
            Mvx.RegisterSingleton(Acr.UserDialogs.UserDialogs.Instance);
        }
    }
}