using System;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;


namespace Acr.UserDialogs.MvvmCross {

    public class PluginLoader : IMvxPluginLoader {
        bool loaded;


        public void EnsureLoaded() {
            if (this.loaded)
                return;

            this.loaded = true;
            Mvx.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);
        }
    }
}
