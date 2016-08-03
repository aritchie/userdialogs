using System;
using Acr.UserDialogs.Builders;
using Android.Content;
using Android.Views;


namespace Acr.UserDialogs.Fragments
{
    public class ConfirmDialogFragment : AbstractBuilderDialogFragment<ConfirmConfig, ConfirmBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            switch (args.KeyCode)
            {
                case Keycode.Back:
                case Keycode.Enter:
                    args.Handled = true;
                    this.Config?.OnConfirm(args.KeyCode == Keycode.Enter);
                    this.Dismiss();
                    break;
            }
            base.OnKeyPress(sender, args);
        }
    }


    public class ConfirmAppCompatDialogFragment : AbstractBuilderAppCompatDialogFragment<ConfirmConfig, ConfirmBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            switch (args.KeyCode)
            {
                case Keycode.Back:
                case Keycode.Enter:
                    args.Handled = true;
                    this.Config?.OnConfirm(args.KeyCode == Keycode.Enter);
                    this.Dismiss();
                    break;
            }
            base.OnKeyPress(sender, args);
        }
    }
}