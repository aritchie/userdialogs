using System;
using Windows.UI;


namespace Acr.UserDialogs
{
    public static class Extensions
    {
        public static Windows.UI.Color ToNative(this System.Drawing.Color This)
            => Windows.UI.Color.FromArgb(This.A, This.R, This.G, This.B);
    }
}
