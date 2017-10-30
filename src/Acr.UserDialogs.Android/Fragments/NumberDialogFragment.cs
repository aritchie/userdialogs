using System;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Content;
using Android.Views;

namespace Acr.UserDialogs.Fragments
{
    public class NumberDialogFragment : AbstractDialogFragment<NumberPromptConfig>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            if (this.Config.IsCancellable)
            {
                this.Config?.OnAction?.Invoke(new NumberPromptResult(false, 0));
                this.Dismiss();
            }
        }


        protected override void SetDialogDefaults(Dialog dialog)
        {
            base.SetDialogDefaults(dialog);
            dialog.Window.SetSoftInputMode(SoftInput.StateHidden);
        }


        protected override Dialog CreateDialog(NumberPromptConfig config)
        {
            return new NumberPromptBuilder().Build(this.Activity, config);
        }
    }

    public class NumberAppCompatDialogFragment : AbstractAppCompatDialogFragment<NumberPromptConfig>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            if (this.Config.IsCancellable)
            {
                this.Config?.OnAction?.Invoke(new NumberPromptResult(false, 0));
                this.Dismiss();
            }
        }


        protected override void SetDialogDefaults(Dialog dialog)
        {
            dialog.SetCancelable(false);
            dialog.SetCanceledOnTouchOutside(false);
            dialog.KeyPress += this.OnKeyPress;
        }


        protected override Dialog CreateDialog(NumberPromptConfig config)
        {
            return new NumberPromptBuilder().Build(this.AppCompatActivity, config);
        }
    }
}
