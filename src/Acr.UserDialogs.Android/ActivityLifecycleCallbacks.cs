using System;
using Android.App;
using Android.OS;


namespace Acr.UserDialogs {

    public class ActivityLifecycleCallbacks : Java.Lang.Object, Application.IActivityLifecycleCallbacks {

        static Activity currentActivity;
        public static Activity CurrentTopActivity {
            get { return currentActivity ?? Application.Context as Activity; }
            protected set { currentActivity = value; }
        }


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