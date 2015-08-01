using System;
using Android.App;
using Android.OS;


namespace Acr.UserDialogs {

    public class ActivityLifecycleCallbacks : Java.Lang.Object, Application.IActivityLifecycleCallbacks {

        public static void Register(Activity activity) {
            activity.Application.RegisterActivityLifecycleCallbacks(new ActivityLifecycleCallbacks());
            CurrentTopActivity = activity;
        }

        public static Activity CurrentTopActivity { get; protected set; }


        public virtual void OnActivityCreated(Activity activity, Bundle savedInstanceState) {
            CurrentTopActivity = activity;
        }


        public virtual void OnActivityResumed(Activity activity) {
            CurrentTopActivity = activity;
        }


        public virtual void OnActivityPaused(Activity activity) {}
        public virtual void OnActivityDestroyed(Activity activity) {}
        public virtual void OnActivitySaveInstanceState(Activity activity, Bundle outState) {}
        public virtual void OnActivityStarted(Activity activity) {}
        public virtual void OnActivityStopped(Activity activity) {}
    }
}