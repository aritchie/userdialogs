using System;
using Android.App;
using Android.Arch.Lifecycle;
using Android.Content;


namespace Acr.UserDialogs
{
    public class AcrLifecycleObserver : Java.Lang.Object, ILifecycleObserver
    {
        readonly Context context;
        readonly Lifecycle lifecycle;


        public AcrLifecycleObserver(Context context, Lifecycle lifecycle)
        {
            this.lifecycle = lifecycle;
            this.context = context;
        }


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