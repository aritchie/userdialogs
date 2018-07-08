using System;
using AppKit;

namespace Acr.UserDialogs.Platforms.macOS
{

    public static partial class UserDialogs
    {
        /// <summary>
        /// OPTIONAL: Initialize MacOS user dialogs with your top viewcontroll function
        /// </summary>
        public static void Init(Func<NSViewController> viewControllerFunc)
        {
            Instance = new UserDialogsImpl(viewControllerFunc);
        }


        static IUserDialogs currentInstance;
        public static IUserDialogs Instance
        {
            get
            {
                currentInstance = currentInstance ?? new UserDialogsImpl();
                return currentInstance;
            }
            set => currentInstance = value;
        }
    }
}
