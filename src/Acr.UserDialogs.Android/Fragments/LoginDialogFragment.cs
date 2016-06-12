using System;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Support.V7.App;


namespace Acr.UserDialogs.Fragments
{
    public class LoginDialogFragment : AbstractBuilderDialogFragment<LoginConfig, LoginBuilder>
    {
    }


    public class LoginAppCompatDialogFragment : AbstractBuilderAppCompatDialogFragment<LoginConfig, LoginBuilder>
    {
    }
}