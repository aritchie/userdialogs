using System;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;


namespace Acr.MvvmCross.Plugins.UserDialogs.WinPhone {

    public class Plugin : IMvxPlugin {

        public void Load() {
            Mvx.RegisterSingleton(Acr.UserDialogs.UserDialogs.Instance);
        }
    }
}