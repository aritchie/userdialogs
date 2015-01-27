using System;
#if __ANDROID__
using Android.App;
#endif


namespace Acr.UserDialogs {

    public static class UserDialogs {


#if __ANDROID__
        public static void Init(Func<Activity> getActivity) {
            if (Instance == null)
                Instance = new UserDialogsImpl(getActivity);
        }


        public static void Init(Activity activity) {
            if (Instance != null)
                return;
            var app = Application.Context.ApplicationContext as Application;
            if (app == null)
                throw new Exception("Application Context is not an application");

            ActivityMonitor.CurrentTopActivity = activity;
            app.RegisterActivityLifecycleCallbacks(new ActivityMonitor());

            Instance = new UserDialogsImpl(() => ActivityMonitor.CurrentTopActivity);
        }
#else
        public static void Init() {
#if __PLATFORM__
            if (Instance == null)
                Instance = new UserDialogsImpl();
#endif
        }
#endif


        public static IUserDialogs Instance { get; set; }
    }
}
