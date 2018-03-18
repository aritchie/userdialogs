using System;
using ElmSharp;


namespace Acr.UserDialogs
{
    public static partial class UserDialogs
    {
        /// <summary>
        /// Initialize Tizen user dialogs
        /// </summary>
        /// <param name="window"></param>
        public static void Init(Window window)
        {
            Instance = new UserDialogsImpl(window);
        }


        static IUserDialogs currentInstance;
        public static IUserDialogs Instance
        {
            get
            {
                if (currentInstance == null)
                    throw new ArgumentException("[Acr.UserDialogs] In Tizen, the window instance of your custom application must be passed by using UserDialogs.Init(Window).");

                return currentInstance;
            }
            set => currentInstance = value;
        }
    }
}
