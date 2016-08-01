using System;
using Acr.UserDialogs.Builders;
using Android.Content;


namespace Acr.UserDialogs.Fragments
{
    public class LoginDialogFragment : AbstractBuilderDialogFragment<LoginConfig, LoginBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            this.Config?.OnResult(new LoginResult(false, null, null));
            base.OnKeyPress(sender, args);
        }
    }


    public class LoginAppCompatDialogFragment : AbstractBuilderAppCompatDialogFragment<LoginConfig, LoginBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            this.Config?.OnResult(new LoginResult(false, null, null));
            base.OnKeyPress(sender, args);
        }
    }
}