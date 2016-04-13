using System;
using Android.App;
using Android.Support.V7.App;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;


namespace Acr.UserDialogs.Builders
{
    public static class ConfirmBuilder
    {
        public static AlertDialog.Builder Build(Activity activity, ConfirmConfig config)
        {
            return new AlertDialog
                .Builder(activity)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (s, a) => config.OnConfirm(true))
                .SetNegativeButton(config.CancelText, (s, a) => config.OnConfirm(false));
        }


        public static AppCompatAlertDialog.Builder Build(AppCompatActivity activity, ConfirmConfig config)
        {
            return new AppCompatAlertDialog
                .Builder(activity)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (s, a) => config.OnConfirm(true))
                .SetNegativeButton(config.CancelText, (s, a) => config.OnConfirm(false));
        }
    }
}