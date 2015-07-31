using System;
#if __ANDROID__
using Android.App;
#endif


namespace Acr.UserDialogs {

    public static class UserDialogs {

#if __ANDROID__
        /// <summary>
        /// You need to provide a factory function
        /// </summary>
        public static void Init(bool useMaterialDesign = false) {
            if (Instance != null)
                return;

            ((Application)Application.Context).RegisterActivityLifecycleCallbacks(new ActivityLifecycleCallbacks());
            Instance = new UserDialogsImpl(() => ActivityLifecycleCallbacks.CurrentTopActivity);
        }
#elif PCL
        [Obsolete("You must call the Init() method from the platform project, not this PCL version")]
        public static void Init() {
			throw new ArgumentException("You must call the Init() method from the platform project, not this PCL version");
        }
#else
        public static void Init() {
			Instance = new UserDialogsImpl();
        }
#endif

        public static IUserDialogs Instance { get; set; }
    }
}
