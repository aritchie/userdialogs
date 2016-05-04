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
                (sender, args) => dateTime = args.Date,
                dateTime.Year,
                dateTime.Month + 1,
                dateTime.Day
            );
            if (!String.IsNullOrWhiteSpace(config.Title))
                dialog.SetTitle(config.Title);

            if (config.MinimumDate != null)
                dialog.DatePicker.MinDate = config.MinimumDate.Value.ToUnixTimestamp();

            if (config.MaximumDate != null)
                dialog.DatePicker.MaxDate = config.MaximumDate.Value.ToUnixTimestamp();

            dialog.SetCancelable(config.IsCancellable);
            if (config.IsCancellable)
            {
                dialog.SetButton(
                    (int) DialogButtonType.Negative,
                    new SpannableString(config.CancelText),
                    (sender, args) => config.OnResult?.Invoke(new DatePromptResult(false, dateTime))
                );
            }
            dialog.SetButton(
                (int)DialogButtonType.Positive,
                new SpannableString(config.OkText),
                (sender, args) => config.OnResult?.Invoke(new DatePromptResult(true, dateTime))
            );
            // hook these, not called by fragments though
            dialog.DismissEvent += (sender, args) => config.OnResult?.Invoke(new DatePromptResult(true, dateTime));
            dialog.CancelEvent += (sender, args) => config.OnResult?.Invoke(new DatePromptResult(false, dateTime));

            return dialog;
        }


        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            var utc = TimeZoneInfo.ConvertTimeToUtc(dateTime);
            return Convert.ToInt64((utc - Epoch).TotalSeconds);
        }
    }
}