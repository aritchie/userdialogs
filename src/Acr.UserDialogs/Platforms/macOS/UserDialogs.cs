using System;
using AppKit;


namespace Acr.UserDialogs
{
    public static partial class UserDialogs
    {
        /// <summary>
        /// OPTIONAL: Initialize macOS user dialogs with your top window function
        /// </summary>
        public static void Init(Func<NSWindow> windowFunc)
        {
            Instance = new UserDialogsImpl(windowFunc);
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
