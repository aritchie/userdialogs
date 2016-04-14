using System;
using Android.App;


namespace Acr.UserDialogs.Builders
{
    public static class DateTimeBuilder
    {

        public static DatePickerDialog BuildDatePicker(Activity activity, DateTimePromptConfig config)
        {
            var dateTime = config.SelectedDateTime ?? DateTime.Now;
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

            //if (config.IsCancellable)
                //dialog.Set

            // hook these, not called by fragments though
            dialog.DismissEvent += (sender, args) => config.OnResult?.Invoke(new DateTimePromptResult(true, dateTime));
            dialog.CancelEvent += (sender, args) => config.OnResult?.Invoke(new DateTimePromptResult(false, dateTime));

            return dialog;
        }


        public static TimePickerDialog BuildTimePicker(Activity activity, DateTimePromptConfig config)
        {
            var dateTime = config.SelectedDateTime ?? DateTime.Now;
            var dialog = new TimePickerDialog(
                activity,
                (sender, args) => dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, args.HourOfDay, args.Minute, 0),
                dateTime.Hour,
                dateTime.Minute,
                false
            );
            if (!String.IsNullOrWhiteSpace(config.Title))
                dialog.SetTitle(config.Title);

            // hook these, not called by fragments though
            dialog.DismissEvent += (sender, args) => config.OnResult?.Invoke(new DateTimePromptResult(true, dateTime));
            dialog.CancelEvent += (sender, args) => config.OnResult?.Invoke(new DateTimePromptResult(false, dateTime));

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