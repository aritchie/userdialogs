using System;
using Android.App;
using Android.Content;
using Android.Text;


namespace Acr.UserDialogs.Builders
{
    public static class TimePromptBuilder
    {
        public static TimePickerDialog Build(Activity activity, TimePromptConfig config)
        {
            var time = config.SelectedTime ?? DateTime.Now.TimeOfDay;
            var dateTime = DateTime.Now;

            var dialog = new TimePickerDialog(
                activity,
                (sender, args) => dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, args.HourOfDay, args.Minute, 0),
                time.Hours,
                time.Minutes,
                false
            );
            if (!String.IsNullOrWhiteSpace(config.Title))
                dialog.SetTitle(config.Title);

            dialog.SetCancelable(config.IsCancellable);
            if (config.IsCancellable)
            {
                dialog.SetButton(
                    (int) DialogButtonType.Negative,
                    new SpannableString(config.CancelText),
                    (sender, args) => config.OnResult?.Invoke(new TimePromptResult(false, dateTime.TimeOfDay))
                );
            }
            dialog.SetButton(
                (int)DialogButtonType.Positive,
                new SpannableString(config.OkText),
                (sender, args) => config.OnResult?.Invoke(new TimePromptResult(true, dateTime.TimeOfDay))
            );
            // hook these, not called by fragments though
            dialog.DismissEvent += (sender, args) => config.OnResult?.Invoke(new TimePromptResult(true, dateTime.TimeOfDay));
            dialog.CancelEvent += (sender, args) => config.OnResult?.Invoke(new TimePromptResult(false, dateTime.TimeOfDay));

            return dialog;
        }
    }
}