using System;
#if __ANDROID__
using Android.App;
#endif


namespace Acr.UserDialogs {

    public static class UserDialogs {
        private static bool init;


#if __ANDROID__
        public static void Init(Func<Activity> getActivity) {
            if (init)
                return;

            init = true;
            Instance = new UserDialogsImpl(getActivity);
        }


        public static void Init(Activity activity) {
            var app = Application.Context.ApplicationContext as Application;
            if (app == null)
                throw new Exception("Application Context is not an application");

            ActivityMonitor.CurrentTopActivity = activity;
            app.RegisterActivityLifecycleCallbacks(new ActivityMonitor());

            Instance = new UserDialogsImpl(() => ActivityMonitor.CurrentTopActivity);
        }
#else
        public static void Init() {
            if (init)
                return;

            init = true;
#if __PLATFORM__
            Instance = new UserDialogsImpl();
#endif
        }
#endif


        public static IUserDialogs Instance { get; set; }
    }
}
