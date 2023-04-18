﻿using System;
using Android.App;
using Android.Graphics.Drawables;
using Android.Util;
#if ANDROIDX
using AndroidX.Core.Content;
#else
using Android.Support.V4.Content;
#endif

namespace Acr.UserDialogs.Infrastructure
{
    public static class ImageLoader
    {
        public static Drawable Load(string resourceName)
        {
            var con = Application.Context;
            var res = con.Resources;

            if (resourceName.StartsWith("assets/", StringComparison.CurrentCultureIgnoreCase))
            {
                // load from stream
                var asset = res.Assets.Open(resourceName);
                return Drawable.CreateFromResourceStream(res, new TypedValue(), asset, null);
            }

            var index = resourceName.LastIndexOf(".");
            if (index > -1)
                resourceName = resourceName.Substring(0, index);

            var resourceId = res.GetIdentifier(resourceName, "drawable", con.PackageName);
            if (resourceId == 0)
                return null;
            return ContextCompat.GetDrawable(Application.Context, resourceId);
        }
    }
}
