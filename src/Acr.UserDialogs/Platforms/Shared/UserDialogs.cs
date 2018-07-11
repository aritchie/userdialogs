﻿using System;
using Acr.UserDialogs.Infrastructure;
#if __IOS__
using UIKit;
#endif
#if __MACOS__
using AppKit;
#endif
#if __ANDROID__
using Android.App;
#endif
#if WINDOWS_UWP
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
#endif
#if __TIZEN__
using ElmSharp;
#endif

namespace Acr.UserDialogs
{

    public static class UserDialogs
    {

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
#elif __IOS__

        /// <summary>
        /// OPTIONAL: Initialize iOS user dialogs with your top viewcontroll function
        /// </summary>
        public static void Init(Func<UIViewController> viewControllerFunc)
        {
            Instance = new UserDialogsImpl(viewControllerFunc);
        }
#elif __MACOS__

        /// <summary>
        /// OPTIONAL: Initialize iOS user dialogs with your top viewcontroll function
        /// </summary>
        public static void Init(Func<NSViewController> viewControllerFunc)
        {
            Instance = new UserDialogsImpl(viewControllerFunc);
        }
#elif __TIZEN__
        /// <summary>
        /// Initialize Tizen user dialogs
        /// </summary>
        /// <param name="window"></param>
        public static void Init(Window window)
        {
            Instance = new UserDialogsImpl(window);
        }
#elif WINDOWS_UWP
/// <summary>
/// Initialize UWP user dialogs
/// </summary>
        public static void Init(Func<Action, Task> customDispatcher = null)
        {
            Instance = new UserDialogsImpl(customDispatcher);
        }
#endif

        static IUserDialogs currentInstance;
        public static IUserDialogs Instance
        {
            get
            {
#if BAIT
                if (currentInstance == null)
                    throw new ArgumentException("[Acr.UserDialogs] This is the bait library, not the platform library.  You must install the nuget package in your main executable/application project");
#elif __ANDROID__
                if (currentInstance == null)
                    throw new ArgumentException("[Acr.UserDialogs] In android, you must call UserDialogs.Init(Activity) from your first activity OR UserDialogs.Init(App) from your custom application OR provide a factory function to get the current top activity via UserDialogs.Init(() => supply top activity)");
#elif __TIZEN__
                if (currentInstance == null)
                {
                    throw new ArgumentException("[Acr.UserDialogs] In Tizen, the window instance of your custom application must be passed by using UserDialogs.Init(Window).");
                }
#else
                currentInstance = currentInstance ?? new UserDialogsImpl();
#endif
                return currentInstance;
            }
            set => currentInstance = value;
        }
    }
}
