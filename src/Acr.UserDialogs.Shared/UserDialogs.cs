using System;
#if __ANDROID__
using Android.App;
#endif


namespace Acr.UserDialogs {

    public static class UserDialogs {

#if __ANDROID__
        public static void Init(Func<Activity> getActivity) {
            Instance = new UserDialogsImpl(getActivity);
        }

#elif __PLATFORM__
        public static void Init() {
			Instance = new UserDialogsImpl();
        }
#else
        [Obsolete("You must call the Init() method from the platform project, not this PCL version")]
        public static void Init() {
			throw new ArgumentException("You must call the Init() method from the platform project, not this PCL version");
        }
#endif

        public static IUserDialogs Instance { get; set; }


		internal static void TryExecute(this Action action) {
			if (action != null)
				action();
		}
    }
}
