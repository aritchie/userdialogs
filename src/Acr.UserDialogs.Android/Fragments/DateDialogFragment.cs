using System;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Content;
using Android.Views;


namespace Acr.UserDialogs.Fragments
{
    public class DateDialogFragment : AbstractDialogFragment<DatePromptConfig>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            this.Config?.OnResult?.Invoke(new DatePromptResult(false, DateTime.MinValue));
            this.Dismiss();
        }


        protected override Dialog CreateDialog(DatePromptConfig config)
        {
            return DatePromptBuilder.Build(this.Activity, config);
        }
    }


    public class DateAppCompatDialogFragment : AbstractAppCompatDialogFragment<DatePromptConfig>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            this.Config?.OnResult?.Invoke(new DatePromptResult(false, DateTime.MinValue));
            this.Dismiss();
        }


        protected override Dialog CreateDialog(DatePromptConfig config)
        {
            return DatePromptBuilder.Build(this.Activity, config);
        }
    }
}