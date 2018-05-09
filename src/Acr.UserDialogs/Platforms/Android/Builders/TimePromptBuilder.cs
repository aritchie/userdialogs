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
            var themeId = config.AndroidStyleId ?? 0;
            var is24Hour = config.Use24HourClock ?? DateFormat.Is24HourFormat(activity);

            var timePickerDialog = new TimePickerDialog(activity, themeId, (sender, args) =>
              {
                  var ts = new TimeSpan(0, args.HourOfDay, args.Minute, 0);
                  config.OnAction?.Invoke(new TimePromptResult(true, ts));
              }, config.SelectedTime.Value.Hours, config.SelectedTime.Value.Minutes, is24Hour);

            return timePickerDialog;
        }
    }
}