using System;


namespace Acr.UserDialogs {

    public static class UserDialogs {
        private static bool init;


#if __ANDROID__
        public static void Init(Android.App.Activity activity) {
            if (init)
                return;

            init = true;
            Instance = new UserDialogsImpl(activity);
        }
#else
        public static void Init() {
            init = true;
#if __PLATFORM__
            Instance = new UserDialogsImpl();
#endif
        }
#endif


        public static IUserDialogs Instance { get; set; }
    }
}
