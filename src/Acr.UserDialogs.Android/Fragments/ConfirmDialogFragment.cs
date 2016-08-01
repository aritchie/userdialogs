using System;
using Acr.UserDialogs.Builders;
using Android.Content;


namespace Acr.UserDialogs.Fragments
{
    public class ConfirmDialogFragment : AbstractBuilderDialogFragment<ConfirmConfig, ConfirmBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            this.Config?.OnConfirm(false);
            base.OnKeyPress(sender, args);
        }
    }


    public class ConfirmAppCompatDialogFragment : AbstractBuilderAppCompatDialogFragment<ConfirmConfig, ConfirmBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            this.Config?.OnConfirm(false);
            base.OnKeyPress(sender, args);
        }
    }
}