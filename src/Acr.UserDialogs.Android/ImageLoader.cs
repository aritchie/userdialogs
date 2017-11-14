using System;
using Android.App;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;


namespace Acr.UserDialogs
{
    public static class ImageLoader
    {
        public static Drawable Load(string resourceName)
        {
            // TODO: from assets or drawables?
            var resourceId = Application.Context.Resources.GetIdentifier(resourceName, "drawable", Application.Context.PackageName);
            return ContextCompat.GetDrawable(Application.Context, resourceId);
        }
    }
}