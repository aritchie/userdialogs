using System;
#if __ANDROID__
using Android.App;
using Acr.Support.Android;
#endif

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
        /// Initialize android user dialogs.  DO NOT use appcompat if you don't know what it is or how to use it.  You will not get pretty toasts or material design
        /// </summary>
        public static void Init(Func<Activity> topActivityFactory, bool useAppCompat = false) {
            if (useAppCompat)
                Instance = new AppCompatUserDialogsImpl(topActivityFactory);
            else
                Instance = new UserDialogsImpl(topActivityFactory);
        }


        /// <summary>
        /// Initialize android user dialogs. DO NOT use appcompat if you don't know what it is or how to use it.  You will not get pretty toasts or material design
        /// </summary>
        public static void Init(Application app, bool useAppCompat = false) {
            ActivityLifecycleCallbacks.Register(app);
            Init((() => ActivityLifecycleCallbacks.CurrentTopActivity), useAppCompat);
        }


        /// <summary>
        /// Initialize android user dialogs. DO NOT use appcompat if you don't know what it is or how to use it.  You will not get pretty toasts or material design
        /// </summary>
        public static void Init(Activity activity, bool useAppCompat = false) {
            ActivityLifecycleCallbacks.Register(activity);
            Init((() => ActivityLifecycleCallbacks.CurrentTopActivity), useAppCompat);
        }
#endif

        static IUserDialogs customInstance;
        public static IUserDialogs Instance {
            get { return customInstance ?? instance.Value; }
            set { customInstance = value; }
        }
    }
}
