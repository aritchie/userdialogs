using System;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Content;
using Android.Views;


namespace Acr.UserDialogs.Fragments
{
    public class DateAppCompatDialogFragment : AbstractAppCompatDialogFragment<DatePromptConfig>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            if (this.Config.IsCancellable)
            {
                this.Config?.OnAction?.Invoke(new DatePromptResult(false, DateTime.MinValue));
                this.Dismiss();
            }
        }


        protected override void SetDialogDefaults(Dialog dialog)
        {
            dialog.SetCancelable(false);
            dialog.SetCanceledOnTouchOutside(false);
            dialog.KeyPress += this.OnKeyPress;
        }


        protected override Dialog CreateDialog(DatePromptConfig config)
        {
            return DatePromptBuilder.Build(this.AppCompatActivity, config);
        }
    }
}