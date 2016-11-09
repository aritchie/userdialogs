using System;
using Acr.UserDialogs.Builders;
using Android.App;


namespace Acr.UserDialogs.Fragments
{
    public class LoginDialogFragment : AbstractDialogFragment<LoginConfig>
    {
        protected override Dialog CreateDialog(LoginConfig config)
        {
            return new LoginBuilder().Build(this.Activity, config);
        }
    }


    public class LoginAppCompatDialogFragment : AbstractAppCompatDialogFragment<LoginConfig>
    {
        protected override Dialog CreateDialog(LoginConfig config)
        {
            return new LoginBuilder().Build(this.Activity, config);
        }
    }
}