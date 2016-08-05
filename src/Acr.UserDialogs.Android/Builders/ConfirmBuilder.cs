using System;
using Android.App;
using Android.Support.V7.App;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;


namespace Acr.UserDialogs.Builders
{
    public class ConfirmBuilder : AbstractAlertDialogBuilder<ConfirmConfig>
    {
        public override AlertDialog.Builder Build(Activity activity, ConfirmConfig config)
        {
            return this
                .CreateBaseBuilder(activity, config.AndroidStyleId)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (s, a) => config.OnAction(true))
                .SetNegativeButton(config.CancelText, (s, a) => config.OnAction(false));
        }


        public override AppCompatAlertDialog.Builder Build(AppCompatActivity activity, ConfirmConfig config)
        {
            return this
                .CreateBaseBuilder(activity, config.AndroidStyleId)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (s, a) => config.OnAction(true))
                .SetNegativeButton(config.CancelText, (s, a) => config.OnAction(false));
        }
    }
}