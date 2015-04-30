using System;
#if __ANDROID__
using Android.App;
#endif


namespace Acr.UserDialogs {

    public static class UserDialogs {
        private static readonly Lazy<IUserDialogs> instanceInit = new Lazy<IUserDialogs>(() => {
#if __ANDROID__
            if (getActivity == null)
                throw new ArgumentException("Android requires that you pass an activity factory function to Init() from your main activity");
            return new UserDialogsImpl(getActivity);
#elif __PLATFORM__
            return new UserDialogsImpl();
#else
            throw new ArgumentException("No platform implementation found.  Did you install this package into your application project?");
#endif
        }, false);

#if __ANDROID__
        private static Func<Activity> getActivity;
        public static void Init(Func<Activity> activityFactory) {
            getActivity = activityFactory;
        }

#endif


        private static IUserDialogs customInstance;
        public static IUserDialogs Instance {
            get { return customInstance ?? instanceInit.Value; }
            set { customInstance = value; }
        }


		internal static void TryExecute(this Action action) {
			if (action != null)
				action();
		}
    }
}
