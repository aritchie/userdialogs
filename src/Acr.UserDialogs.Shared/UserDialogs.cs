using System;


namespace Acr.UserDialogs {

    public static class UserDialogs {

        static readonly Lazy<IUserDialogs> instance = new Lazy<IUserDialogs>(() => {
#if PCL
            throw new ArgumentException("This PCL library, not the platform library.  Did you include the nuget package in your project?");
#elif __ANDROID__
            throw new ArgumentException("In android, you must call UserDialogs.Init(Activity) from your first activity");
#else
            return new UserDialogsImpl();
#endif
        });

#if __ANDROID__


        /// <summary>
        /// Initialize android user dialogs
        /// </summary>
        public static void Init(Android.App.Activity activity, bool useAppCompat = false) {
            ActivityLifecycleCallbacks.Register(activity);
            if (useAppCompat)
                Instance = new AppCompatUserDialogsImpl(null);
            else
                Instance = new UserDialogsImpl(null);
        }
#endif

        static IUserDialogs customInstance;
        public static IUserDialogs Instance {
            get { return customInstance ?? instance.Value; }
            set { customInstance = value; }
        }
    }
}
