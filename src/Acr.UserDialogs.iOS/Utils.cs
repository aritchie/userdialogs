using System;
using System.Linq;
using UIKit;


namespace Acr.UserDialogs {

    public static class Utils {

        public static UIWindow GetTopWindow() {
            return UIApplication.SharedApplication
                .Windows
                .Reverse()
                .FirstOrDefault(x => 
                    x.WindowLevel == UIWindowLevel.Normal && 
                    !x.Hidden
                );
        }


        public static UIView GetTopView() {
            return GetTopWindow().Subviews.Last();
        }


        public static UIViewController GetTopViewController() {
            var root = GetTopWindow().RootViewController;
            var tabs = root as UITabBarController;
            if (tabs != null)
                return tabs.SelectedViewController;

            var nav = root as UINavigationController;
            if (nav != null)
                return nav.VisibleViewController;

            if (root.PresentedViewController != null)
                return root.PresentedViewController;

            return root;
        }


        public static void SetInputType(UITextField txt, InputType inputType) {
            switch (inputType) {
                case InputType.Email  :
                    txt.KeyboardType = UIKeyboardType.EmailAddress;
                    break;

                case InputType.Number: 
                    txt.KeyboardType = UIKeyboardType.NumberPad;
                    break;

                case InputType.Password:
                    txt.SecureTextEntry = true;
                    break;

                default :
                    txt.KeyboardType = UIKeyboardType.Default;
                    break;
            }
        }
    }
}