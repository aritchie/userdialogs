using System;
using Android.App;
using Android.Content;
using Android.Text;


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
                dateTime.Month + 1,
                dateTime.Day
            );
            dialog.SetCancelable(false);

            if (!String.IsNullOrWhiteSpace(config.Title))
                dialog.SetTitle(config.Title);

            if (config.MinimumDate != null)
                dialog.DatePicker.MinDate = config.MinimumDate.Value.ToUnixTimestamp();

            if (config.MaximumDate != null)
                dialog.DatePicker.MaxDate = config.MaximumDate.Value.ToUnixTimestamp();

            if (config.IsCancellable)
            {
                dialog.SetButton(
                    (int) DialogButtonType.Negative,
                    new SpannableString(config.CancelText),
                    (sender, args) =>
                    {
                        config.OnResult?.Invoke(new DatePromptResult(false, dialog.DatePicker.DateTime.Date));
                    }
                );
            }
            dialog.SetButton(
                (int)DialogButtonType.Positive,
                new SpannableString(config.OkText),
                (sender, args) =>
                {
                    config.OnResult?.Invoke(new DatePromptResult(true, dialog.DatePicker.DateTime.Date));
                }
            );
            return dialog;
        }


        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
        }
    }
}