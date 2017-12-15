using System;
using Windows.UI;


namespace Acr.UserDialogs
{
    public static class Extensions
    {
        public static Color ToNative(this System.Drawing.Color This) => Color.FromArgb(This.A, This.R, This.G, This.B);
    }
}
