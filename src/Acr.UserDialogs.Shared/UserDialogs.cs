using System;
#if __ANDROID__
using Android.App;
using Acr.Support.Android;
#endif
#if __IOS__
using UIKit;
#endif

namespace Acr.UserDialogs {

    public static class UserDialogs {

        static readonly Lazy<IUserDialogs> instance = new Lazy<IUserDialogs>(() =>
        {
#if PCL
            throw new ArgumentException("This is the PCL library, not the platform library.  You must install the nuget package in your main executable/application project");
#elif __ANDROID__
            throw new ArgumentException("In android, you must call UserDialogs.Init(Activity) from your first activity OR UserDialogs.Init(App) from your custom application OR provide a factory function to get the current top activity via UserDialogs.Init(() => supply top activity)");
#else
            return new UserDialogsImpl();
#endif
        });

#if __ANDROID__

        /// <summary>
        /// Initialize android user dialogs
        /// </summary>
        public static void Init(Func<Activity> topActivityFactory)
        {
            Instance = new UserDialogsImpl(topActivityFactory);
        }


        /// <summary>
        /// Initialize android user dialogs
        /// </summary>
        public static void Init(Application app)
        {
            ActivityLifecycleCallbacks.Register(app);
            Init(() => ActivityLifecycleCallbacks.CurrentTopActivity);
        }


        /// <summary>
        /// Initialize android user dialogs
        /// </summary>
        public static void Init(Activity activity)
        {
            ActivityLifecycleCallbacks.Register(activity);
            Init(() => ActivityLifecycleCallbacks.CurrentTopActivity);
        }
#endif

#if __IOS__

        /// <summary>
        /// Initialize iOS user dialogs
        /// </summary>
        public static void Init(Func<UIViewController> viewControllerFunc)
        {
            Instance = new UserDialogsImpl(viewControllerFunc);
        }

        /// <summary>
        /// Initialize iOS user dialogs
        /// </summary>
        public static void Init(UIViewController viewController)
        {
            Instance = new UserDialogsImpl(() => viewController);
        }

#endif

        static IUserDialogs customInstance;
        public static IUserDialogs Instance
        {
            get { return customInstance ?? instance.Value; }
            set { customInstance = value; }
        }
    }
}
