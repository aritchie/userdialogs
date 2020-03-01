using System;
using Android.App;
using AlertDialog = Android.App.AlertDialog;
#if ANDROIDX
using AndroidX.AppCompat.App;
using AppCompatAlertDialog = AndroidX.AppCompat.App.AlertDialog;
#else
using Android.Support.V7.App;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;
#endif

namespace Acr.UserDialogs.Builders
{
    public class ConfirmBuilder : IAlertDialogBuilder<ConfirmConfig>
    {
        public Dialog Build(Activity activity, ConfirmConfig config)
        {
            return new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (s, a) => config.OnAction(true))
                .SetNegativeButton(config.CancelText, (s, a) => config.OnAction(false))
                .Create();
        }


        public Dialog Build(AppCompatActivity activity, ConfirmConfig config)
        {
            return new AppCompatAlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (s, a) => config.OnAction(true))
                .SetNegativeButton(config.CancelText, (s, a) => config.OnAction(false))
                .Create();
        }
    }
}