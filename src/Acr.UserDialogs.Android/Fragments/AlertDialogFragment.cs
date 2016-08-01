using System;
using Acr.UserDialogs.Builders;
using Android.Content;


namespace Acr.UserDialogs.Fragments
{
    public class AlertDialogFragment : AbstractBuilderDialogFragment<AlertConfig, AlertBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            this.Config?.OnOk?.Invoke();
            base.OnKeyPress(sender, args);
        }
    }


    public class AlertAppCompatDialogFragment : AbstractBuilderAppCompatDialogFragment<AlertConfig, AlertBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            this.Config?.OnOk?.Invoke();
            base.OnKeyPress(sender, args);
        }
    }
}