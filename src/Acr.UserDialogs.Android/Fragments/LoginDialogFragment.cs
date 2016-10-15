using System;
using Acr.UserDialogs.Builders;
using Android.Content;
using Android.Views;


namespace Acr.UserDialogs.Fragments
{
    public class LoginDialogFragment : AbstractBuilderDialogFragment<LoginConfig, LoginBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
            {
                args.Handled = true;
                return;
            }
            this.Config?.OnAction(new LoginResult(false, null, null));
            this.Dismiss();
        }
    }


    public class LoginAppCompatDialogFragment : AbstractBuilderAppCompatDialogFragment<LoginConfig, LoginBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
            {
                args.Handled = false;
                return;
            }
            args.Handled = true;
            this.Config?.OnAction(new LoginResult(false, null, null));
            this.Dismiss();
        }
    }
}