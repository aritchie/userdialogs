using System;
using Acr.UserDialogs.Builders;
using Android.App;


namespace Acr.UserDialogs.Fragments
{
    public class AlertDialogFragment : AbstractDialogFragment<AlertConfig>
    {
        protected override Dialog CreateDialog(AlertConfig config)
        {
            return new AlertBuilder().Build(this.Activity, config);
        }
    }


    public class AlertAppCompatDialogFragment : AbstractAppCompatDialogFragment<AlertConfig>
    {
        //protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        //{
        //    base.OnKeyPress(sender, args);
        //    if (args.KeyCode != Keycode.Back)
        //        return;

        //    args.Handled = true;
        //    this.Config?.OnAction?.Invoke();
        //    this.Dismiss();
        //}


        protected override Dialog CreateDialog(AlertConfig config)
        {
            return new AlertBuilder().Build(this.Activity, config);
        }
    }
}