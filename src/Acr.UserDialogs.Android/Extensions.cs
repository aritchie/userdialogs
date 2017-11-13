using System;
using Android.App;


namespace Acr.UserDialogs
{
    public static class Extensions
    {
        public static void SafeRunOnUi(this Activity activity, Action action) => activity.RunOnUiThread(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Log.Error("", ex.ToString());
            }
        });


        public static AndroidHUD.MaskType ToNative(this MaskType maskType)
        {
            switch (maskType)
            {
                case MaskType.Black:
                    return AndroidHUD.MaskType.Black;

                case MaskType.Clear:
                    return AndroidHUD.MaskType.Clear;

                case MaskType.Gradient:
                    Console.WriteLine("Warning - Gradient mask type is not supported on android");
                    return AndroidHUD.MaskType.Black;

                case MaskType.None:
                    return AndroidHUD.MaskType.None;

                default:
                    throw new ArgumentException("Invalid Mask Type");
            }
        }
    }
}