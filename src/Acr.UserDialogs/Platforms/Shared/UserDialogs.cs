using System;


namespace Acr.UserDialogs
{
    public static partial class UserDialogs
    {
        #if NETSTANDARD
        static IUserDialogs currentInstance;
        public static IUserDialogs Instance
        {
            get
            {
                if (currentInstance == null)
                    throw new ArgumentException("[Acr.UserDialogs] This is the bait library, not the platform library.  You must install the nuget package in your main executable/application project");

                return currentInstance;
            }
            set => currentInstance = value;
        }
        #endif
    }
}
