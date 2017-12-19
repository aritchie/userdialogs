using System;
using Android.App;
using Android.Content;
using Android.Support.V7.App;
using Android.Text;
using Android.Util;
using Android.Widget;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;
using Android.Content.Res;

namespace Acr.UserDialogs.Builders
{
    public class ConfirmBuilder : IAlertDialogBuilder<ConfirmConfig>
    {
        public Dialog Build(Activity activity, ConfirmConfig config)
        {
            return new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.IsHtmlText ? null : config.Message)
                .SetView(config.IsHtmlText ? GetHtmlTextView(activity, config.Message) : null)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (s, a) => config.OnAction(true))
                .SetNegativeButton(config.CancelText, (s, a) => config.OnAction(false))
                .Create();
        }


        public Dialog Build(AppCompatActivity activity, ConfirmConfig config)
        {
            return new AppCompatAlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.IsHtmlText ? null : config.Message)
                .SetView(config.IsHtmlText ? GetHtmlTextView(activity, config.Message) : null)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (s, a) => config.OnAction(true))
                .SetNegativeButton(config.CancelText, (s, a) => config.OnAction(false))
                .Create();
        }

        ISpanned FromHtml(String html)
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

        TextView GetHtmlTextView(Context context, string message)
        {
            TextView view = new TextView(context);
            view.TextFormatted = FromHtml(message);

            int[] dialogAttributes = new int[] { Android.Resource.Attribute.DialogPreferredPadding };
            TypedArray typedArray = context.ObtainStyledAttributes(dialogAttributes);
            int horizontalPadding = typedArray.GetDimensionPixelSize(0, -1);
            int verticalPadding = (int)ConvertDpToPx(context, 18);

            view.SetPadding(horizontalPadding, verticalPadding, horizontalPadding, verticalPadding);
            return view;
        }

        public static float ConvertDpToPx(Context context, int dp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            float pixels = TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, metrics);
            return pixels;
        }
    }
}