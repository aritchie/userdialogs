using System;
using BigTed;


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
    }
}