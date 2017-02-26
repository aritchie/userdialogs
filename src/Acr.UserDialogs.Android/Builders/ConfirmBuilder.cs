using System;
using Android.App;
using Android.Support.V7.App;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;


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
            Dialog dialog;

            if (config.IsCancelable)
                dialog = new AppCompatAlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (s, a) => config.OnAction(true))
                .SetNegativeButton(config.NotOkText, (s, a) => config.OnAction(false))
                .SetNeutralButton(config.CancelText, (s, a) => config.OnCancel())
                .Create();
            else
                dialog = new AppCompatAlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (s, a) => config.OnAction(true))
                .SetNegativeButton(config.CancelText, (s, a) => config.OnAction(false))
                .Create();

            return dialog;
        }
    }
}