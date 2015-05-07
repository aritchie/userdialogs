using System;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;
#if __UNIFIED__
using UIKit;
#else
using MonoTouch.UIKit;
#endif


namespace Acr.MvvmCross.Plugins.UserDialogs.Touch {

    public class Plugin : IMvxPlugin {

        public void Load() {
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                Mvx.RegisterSingleton<IUserDialogService>(new iOS8DialogService());
            else
                Mvx.RegisterSingleton<IUserDialogService>(new TouchUserDialogService());
        }
    }
}