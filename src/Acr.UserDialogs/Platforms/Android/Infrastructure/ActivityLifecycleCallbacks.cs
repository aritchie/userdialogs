using System;
using Android.App;
using Android.OS;


namespace Acr.UserDialogs.Infrastructure
{
    public class ActivityLifecycleCallbacks : Java.Lang.Object, Application.IActivityLifecycleCallbacks
    {

        public static void Register(Activity activity)
        {
            Register(activity.Application);
            CurrentTopActivity = activity;
        }


        public static void Register(Application app)
        {
            app.RegisterActivityLifecycleCallbacks(new ActivityLifecycleCallbacks());
        }


        public static Activity CurrentTopActivity { get; protected set; }


        public virtual void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CurrentTopActivity = activity;
        }


        public virtual void OnActivityResumed(Activity activity)
        {
            CurrentTopActivity = activity;
        }


        public virtual void OnActivityPaused(Activity activity) { }
        public virtual void OnActivityDestroyed(Activity activity) { }
        public virtual void OnActivitySaveInstanceState(Activity activity, Bundle outState) { }
        public virtual void OnActivityStarted(Activity activity) { }
        public virtual void OnActivityStopped(Activity activity) { }
    }
}