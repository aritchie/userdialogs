using System;
using Acr.UserDialogs.Builders;
using Android.Content;
using Android.Views;


namespace Acr.UserDialogs.Fragments
{
    public class AlertDialogFragment : AbstractBuilderDialogFragment<AlertConfig, AlertBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            this.Config?.OnOk?.Invoke();
            this.Dismiss();
        }
    }


    public class AlertAppCompatDialogFragment : AbstractBuilderAppCompatDialogFragment<AlertConfig, AlertBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            this.Config?.OnOk?.Invoke();
            this.Dismiss();
        }
    }
}