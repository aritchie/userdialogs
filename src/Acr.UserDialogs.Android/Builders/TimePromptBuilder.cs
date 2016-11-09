using System;
using Android.App;
using Android.Text.Format;
using Android.Widget;
using Java.Lang;


namespace Acr.UserDialogs.Builders
{
    public static class TimePromptBuilder
    {
        public static Dialog Build(Activity activity, TimePromptConfig config)
        {
            var picker = new TimePicker(activity);
            var builder = new AlertDialog
                .Builder(activity)
                .SetCancelable(false)
                .SetTitle(config.Title)
                .SetView(picker);

            if (config.SelectedTime != null)
            {
                picker.CurrentHour = new Integer(config.SelectedTime.Value.Hours);
                picker.CurrentMinute = new Integer(config.SelectedTime.Value.Minutes);
            }

            var is24Hour = config.Use24HourClock ?? DateFormat.Is24HourFormat (activity);
            picker.SetIs24HourView(new Java.Lang.Boolean(is24Hour));

            if (config.Neutral.IsVisible)
            {
                builder.SetNegativeButton(
                    config.Neutral.Text,
                    (sender, args) =>
                    {
                        var ts = new TimeSpan(0, picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                        config.OnAction?.Invoke(new DialogResult<TimeSpan>(DialogChoice.Neutral, ts));
                    }
                );
            }
            builder.SetPositiveButton(
                config.Positive.Text,
                (sender, args) =>
                {
                    var ts = new TimeSpan(0, picker.CurrentHour.IntValue(), picker.CurrentMinute.IntValue(), 0);
                    config.OnAction?.Invoke(new DialogResult<TimeSpan>(DialogChoice.Positive, ts));
                }
            );

            return builder.Show();
        }


        /*dialog.SetCancelable(false);
            dialog.SetCanceledOnTouchOutside(false);
            dialog.KeyPress += this.OnKeyPress;
         * dialog.Window.SetSoftInputMode(SoftInput.StateHidden);
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
        */
    }
}