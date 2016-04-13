using System;
using Android.App;
#if APPCOMPAT
using AlertDialog = Android.Support.V7.App.AlertDialog;
#else
using AlertDialog = Android.App.AlertDialog;
#endif

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
    }
}