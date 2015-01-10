using System;
using Android.App;


namespace Acr.UserDialogs {

using System.Threading;

    public static class Utils {

        public static void RequestMainThread(Action action) {
            if (Application.SynchronizationContext == SynchronizationContext.Current)
                action();
            else
                Application.SynchronizationContext.Post(x => {
                    try {
                        action();
                    }
                    catch { }
                }, null);
        }
    }
}