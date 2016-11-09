using System;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Content;
using Android.Views;


namespace Acr.UserDialogs.Fragments
{
    public class TimeDialogFragment : AbstractDialogFragment<TimePromptConfig>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            if (this.Config.IsCancellable)
            {
                this.Config?.OnAction?.Invoke(new TimePromptResult(false, TimeSpan.MinValue));
                this.Dismiss();
            }
        }


        protected override void SetDialogDefaults(Dialog dialog)
        {
            base.SetDialogDefaults(dialog);
            dialog.Window.SetSoftInputMode(SoftInput.StateHidden);
        }


        protected override Dialog CreateDialog(TimePromptConfig config)
        {
            return TimePromptBuilder.Build(this.Activity, config);
        }
    }


    public class TimeAppCompatDialogFragment : AbstractAppCompatDialogFragment<TimePromptConfig>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            if (this.Config.IsCancellable)
            {
                this.Config?.OnAction?.Invoke(new TimePromptResult(false, TimeSpan.MinValue));
                this.Dismiss();
            }
        }


        protected override void SetDialogDefaults(Dialog dialog)
        {
            dialog.SetCancelable(false);
            dialog.SetCanceledOnTouchOutside(false);
            dialog.KeyPress += this.OnKeyPress;
        }


        protected override Dialog CreateDialog(TimePromptConfig config)
        {
            return TimePromptBuilder.Build(this.Activity, config);
        }
    }
}