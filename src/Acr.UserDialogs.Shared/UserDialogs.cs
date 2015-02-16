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
#elif __PLATFORM__
        public static void Init() {
            if (Instance == null)
                Instance = new UserDialogsImpl();
        }
#else
        [Obsolete("You must call the Init() method from the platform project, not this PCL version")]
        public static void Init() {
        }
#endif


        public static IUserDialogs Instance { get; set; }


		internal static void TryExecute(this Action action) {
			if (action != null)
				action();
		}
    }
}
