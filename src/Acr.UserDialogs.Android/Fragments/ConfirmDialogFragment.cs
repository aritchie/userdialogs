using System;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Content;
using Android.Views;


namespace Acr.UserDialogs.Fragments
{
    public class ConfirmDialogFragment : AbstractDialogFragment<ConfirmConfig>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            this.Config?.OnAction?.Invoke(false);
            this.Dismiss();
        }


        protected override Dialog CreateDialog(ConfirmConfig config)
        {
            return new ConfirmBuilder().Build(this.Activity, config);
        }
    }


    public class ConfirmAppCompatDialogFragment : AbstractAppCompatDialogFragment<ConfirmConfig>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            this.Config?.OnAction?.Invoke(false);
            this.Dismiss();
        }


        ConfirmConfig _config;
        protected override Dialog CreateDialog(ConfirmConfig config)
        {
            _config = config;
            return new ConfirmBuilder().Build(this.AppCompatActivity, config);
        }


        public override void OnCancel(IDialogInterface dialog)
        {
            base.OnCancel(dialog);
        }


        protected override void SetDialogDefaults(Dialog dialog)
        {
            base.SetDialogDefaults(dialog);
            if (_config.IsCancelable)
            {
                dialog.SetCancelable(true);
                dialog.SetCanceledOnTouchOutside(true);
            }
        }
    }
}