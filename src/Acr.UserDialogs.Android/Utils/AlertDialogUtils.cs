using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Acr.UserDialogs.Utils
{
    public class AlertDialogUtils
    {
        public static View GetContentView(Context context, string title, string message)
        {
            int[] dialogAttributes = new int[] { Android.Resource.Attribute.DialogPreferredPadding };
            TypedArray typedArray = context.ObtainStyledAttributes(dialogAttributes);
            int horizontalPadding = typedArray.GetDimensionPixelSize(0, -1);
            int verticalPadding = (int)ConvertDpToPx(context, 18);

            LinearLayout contentView = new LinearLayout(context);
            contentView.Orientation = Android.Widget.Orientation.Vertical;

            if (!string.IsNullOrEmpty(title))
            {
                TextView titleView = new TextView(context);
                titleView.SetTextColor(new ColorStateList(new int[][] { new int[] { } }, new int[] { Color.Black }));
                titleView.TextSize = 20;
                titleView.SetTypeface(null, TypefaceStyle.Bold);
                titleView.TextFormatted = FromHtml(title);
                titleView.SetPadding(horizontalPadding, verticalPadding, horizontalPadding, 0);
                contentView.AddView(titleView);
            }

            if (!string.IsNullOrEmpty(message))
            {
                TextView messageView = new TextView(context);
                messageView.SetTextColor(new ColorStateList(new int[][] { new int[] { } }, new int[] { Color.Black }));
                messageView.TextSize = 16;
                messageView.TextFormatted = FromHtml(message);
                messageView.SetPadding(horizontalPadding, verticalPadding, horizontalPadding, verticalPadding);
                contentView.AddView(messageView);
            }

            return contentView;
        }

        static ISpanned FromHtml(String html)
        {
            ISpanned result;
            if (((int)Android.OS.Build.VERSION.SdkInt) >= 24)
            {
                result = Html.FromHtml(html, FromHtmlOptions.ModeLegacy);
            }
            else
            {
                #pragma warning disable CS0618 // Type or member is obsolete
                result = Html.FromHtml(html);
                #pragma warning restore CS0618 // Type or member is obsolete
            }
            return result;
        }

        public static float ConvertDpToPx(Context context, int dp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            float pixels = TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, metrics);
            return pixels;
        }   
    }
}
