using System;
using Android.App;
using Android.Content;
using Android.Text;
using Android.Views;


namespace Acr.UserDialogs.Builders
{
    public static class DatePromptBuilder
    {
        public static DatePickerDialog Build(Activity activity, DatePromptConfig config)
        {
            var dateTime = config.SelectedDate ?? DateTime.Now;
            var dialog = new DatePickerDialog(
                activity,
                (sender, args) => { },
                dateTime.Year,
                dateTime.Month - 1,
                dateTime.Day
            );
            dialog.SetCancelable(false);

            if (!String.IsNullOrWhiteSpace(config.Title))
                dialog.SetTitle(config.Title);

            if (config.MinimumDate != null)
                dialog.DatePicker.MinDate = config.MinimumDate.Value.ToUnixTimestamp();

            if (config.MaximumDate != null)
                dialog.DatePicker.MaxDate = config.MaximumDate.Value.ToUnixTimestamp();

            //if (config.Negative.IsVisible)
            //{
            //    dialog.SetButton(
            //        (int) DialogButtonType.Negative,
            //        new SpannableString(config.Negative.Text),
            //        (sender, args) => config
            //        .OnAction?
            //        .Invoke(new DialogResult<DateTime>(
            //            DialogChoice.Negative,
            //            dialog.DatePicker.DateTime.Date
            //        ))
            //    );
            //}
            if (config.Neutral.IsVisible)
            {
                dialog.SetButton(
                    (int) DialogButtonType.Neutral,
                    new SpannableString(config.Neutral.Text),
                    (sender, args) => config
                        .OnAction?
                        .Invoke(new DialogResult<DateTime>(
                            DialogChoice.Neutral,
                            dialog.DatePicker.DateTime.Date
                        ))
                );
            }
            dialog.SetButton(
                (int)DialogButtonType.Positive,
                new SpannableString(config.Positive.Text),
                (sender, args) => config
                    .OnAction?
                    .Invoke(new DialogResult<DateTime>(
                    DialogChoice.Positive,
                        dialog.DatePicker.DateTime.Date
                    ))
            );
            dialog.Window.SetSoftInputMode(SoftInput.StateHidden);

            return dialog;
        }


        //protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        //{
        //    base.OnKeyPress(sender, args);
        //    if (args.KeyCode != Keycode.Back)
        //        return;

        //    args.Handled = true;
        //    if (this.Config.IsCancellable)
        //    {
        //        this.Config?.OnAction?.Invoke(new DatePromptResult(false, DateTime.MinValue));
        //        this.Dismiss();
        //    }
        //}


        static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            //var utc = TimeZoneInfo.ConvertTimeToUtc(dateTime);
            var utc = dateTime.ToUniversalTime();
            var ms = utc.Subtract(Epoch).TotalMilliseconds;
            return Convert.ToInt64(ms);
        }
    }
}