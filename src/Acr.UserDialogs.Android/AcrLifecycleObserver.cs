using System;
using Android.Arch.Lifecycle;


namespace Acr.UserDialogs
{
    public class AcrLifecycleObserver : Java.Lang.Object, ILifecycleObserver
    {
        [Lifecycle.Event.OnResume]
        public void OnResume()
        {
        }


        [Lifecycle.Event.OnPause]
        public void OnPause()
        {
        }
    }
}