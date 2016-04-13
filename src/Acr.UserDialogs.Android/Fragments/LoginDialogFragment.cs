using System;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Support.V7.App;


namespace Acr.UserDialogs.Fragments
{
    public class LoginDialogFragment : AbstractDialogFragment<LoginConfig>
    {
        protected override Dialog CreateDialog(LoginConfig config)
        {
            return LoginBuilder.Build(this.Activity, config).Create();
        }
    }


    public class LoginAppCompatDialogFragment : AbstractAppCompatDialogFragment<LoginConfig>
    {
        protected override Dialog CreateDialog(LoginConfig config)
        {
            return LoginBuilder.Build(this.Activity as AppCompatActivity, config).Create();
        }
    }
}