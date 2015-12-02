using System;
#if __ANDROID__
using Android.App;
using Acr.Support.Android;
#endif

namespace Acr.UserDialogs {

    public static class UserDialogs {

        static readonly Lazy<IUserDialogs> instance = new Lazy<IUserDialogs>(() => {
#if PCL
            throw new ArgumentException("This is the PCL library, not the platform library.  You must install the nuget package in your main executable/application project");
#elif __ANDROID__
            Init((Application)Application.Context.ApplicationContext);
            return customInstance;
            //throw new ArgumentException("In android, you must call UserDialogs.Init(Activity) from your first activity");
#else
            return new UserDialogsImpl();
#endif
        });

#if __ANDROID__

        /// <summary>
        /// Initialize android user dialogs
        /// </summary>
        public static void Init(Func<Activity> topActivityFactory) {
            Instance = new UserDialogsImpl(topActivityFactory);
        }


        /// <summary>
        /// Initialize android user dialogs
        /// </summary>
        public static void Init(Application app) {
            ActivityLifecycleCallbacks.Register(app);
            Init(() => ActivityLifecycleCallbacks.CurrentTopActivity);
        }


        /// <summary>
        /// Initialize android user dialogs
        /// </summary>
        public static void Init(Activity activity) {
            ActivityLifecycleCallbacks.Register(activity);
            Init(() => ActivityLifecycleCallbacks.CurrentTopActivity);
        }
#endif

        static IUserDialogs customInstance;
        public static IUserDialogs Instance {
            get { return customInstance ?? instance.Value; }
            set { customInstance = value; }
        }
    }
}
