using System;
using System.Linq;
using BigTed;
using UIKit;


namespace Acr.UserDialogs {

    public static class Extensions {

        public static ProgressHUD.MaskType ToNative(this MaskType maskType) {
            switch (maskType) {
                case MaskType.Black    : return ProgressHUD.MaskType.Black;
                case MaskType.Clear    : return ProgressHUD.MaskType.Clear;
                case MaskType.Gradient : return ProgressHUD.MaskType.Gradient;
                case MaskType.None     : return ProgressHUD.MaskType.None;
                default:
                    throw new ArgumentException("Invalid mask type");
            }
        }


        public static UIWindow GetTopWindow() {
            return UIApplication
                .SharedApplication
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
			if (tabs != null) {
				root = tabs.PresentedViewController ?? tabs.SelectedViewController;

				while (root.PresentedViewController != null)
					root = GetTopViewController (root.PresentedViewController);

				return root;
			}

            var nav = root as UINavigationController;
            if (nav != null)
                return nav.VisibleViewController;

            while (root.PresentedViewController != null)
                root = GetTopViewController(root.PresentedViewController);

            return root;
        }


        public static UIViewController GetTopViewController(UIViewController viewController) {
            if (viewController.PresentedViewController != null)
                return GetTopViewController(viewController.PresentedViewController);

            return viewController;
        }
    }
}