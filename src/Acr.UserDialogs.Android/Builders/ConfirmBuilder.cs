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
using Android.Views;
using Android.Graphics;
using Acr.UserDialogs.Utils;

namespace Acr.UserDialogs.Builders
{
    public class ConfirmBuilder : IAlertDialogBuilder<ConfirmConfig>
    {
        public Dialog Build(Activity activity, ConfirmConfig config)
        {
            return new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetView(AlertDialogUtils.GetContentView(activity, config.Title, config.Message))
                .SetPositiveButton(config.OkText, (s, a) => config.OnAction(true))
                .SetNegativeButton(config.CancelText, (s, a) => config.OnAction(false))
                .Create();
        }


        public Dialog Build(AppCompatActivity activity, ConfirmConfig config)
        {
            return new AppCompatAlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetView(AlertDialogUtils.GetContentView(activity, config.Title, config.Message))
                .SetPositiveButton(config.OkText, (s, a) => config.OnAction(true))
                .SetNegativeButton(config.CancelText, (s, a) => config.OnAction(false))
                .Create();
        }
    }
}