using System;
using Android.App;
using Android.Content;
using Android.Text;
using Android.Widget;


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
                picker.Hour = config.SelectedTime.Value.Hours;
                picker.Minute = config.SelectedTime.Value.Minutes;
            }

            if (config.IsCancellable)
            {
                builder.SetNegativeButton(
                    config.CancelText,
                    (sender, args) =>
                    {
                        var ts = new TimeSpan(0, picker.Hour, picker.Minute, 0);
                        config.OnResult?.Invoke(new TimePromptResult(false, ts));
                    }
                );
            }
            builder.SetPositiveButton(
                config.OkText,
                (sender, args) =>
                {
                    var ts = new TimeSpan(0, picker.Hour, picker.Minute, 0);
                    config.OnResult?.Invoke(new TimePromptResult(true, ts));
                }
            );

            return builder.Show();
        }
    }
}