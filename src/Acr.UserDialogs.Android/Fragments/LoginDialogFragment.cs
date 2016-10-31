using System;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Content;
using Android.Views;


namespace Acr.UserDialogs.Fragments
{
    public class LoginDialogFragment : AbstractDialogFragment<LoginConfig>
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


        protected override Dialog CreateDialog(LoginConfig config)
        {
            return new LoginBuilder().Build(this.Activity, config);
        }
    }


    public class LoginAppCompatDialogFragment : AbstractAppCompatDialogFragment<LoginConfig>
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


        protected override Dialog CreateDialog(LoginConfig config)
        {
            return new LoginBuilder().Build(this.Activity, config);
        }
    }
}