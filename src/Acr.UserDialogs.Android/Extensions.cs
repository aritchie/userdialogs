using System;
using Android.Views;


namespace Acr.UserDialogs {

    public static class Extensions {

        public static AndroidHUD.MaskType ToNative(this MaskType maskType) {
            switch (maskType) {
                case MaskType.Black :
                    return AndroidHUD.MaskType.Black;

                case MaskType.Clear :
                    return AndroidHUD.MaskType.Clear;

                case MaskType.Gradient :
                    Console.WriteLine("Warning - Gradient mask type is not supported on android");
                    return AndroidHUD.MaskType.Black;

                case MaskType.None :
                    return AndroidHUD.MaskType.None;

                default:
                    throw new ArgumentException("Invalid Mask Type");
            }
        }

#if APPCOMPAT
        public static void ShowExt(this Android.Support.V7.App.AlertDialog.Builder builder) {
            var dialog = builder.Create();
            dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            dialog.Show();
        }

#else
        public static void ShowExt(this Android.App.AlertDialog.Builder builder) {
            var dialog = builder.Create();
            dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            dialog.Show();
        }
#endif
    }
}