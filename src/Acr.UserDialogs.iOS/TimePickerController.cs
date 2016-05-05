using System;
using Foundation;
using UIKit;


namespace Acr.UserDialogs
{
    public class TimePickerController : ModalDateTimePickerViewController
    {
        public TimePickerController(TimePromptConfig config, UIViewController parent) : base(config.Title, parent)
        {
            this.DatePicker = new UIDatePicker
            {
                Mode = UIDatePickerMode.Time,
                MinuteInterval = config.MinuteInterval
            };
            this.Dismissed += (sender, b) =>
            {
                var time = ((DateTime) this.DatePicker.Date).ToLocalTime().TimeOfDay;
                config.OnResult?.Invoke(new TimePromptResult(b, time));
            };

            this.DoneButtonText = config.OkText;
            if (config.IsCancellable)
            {
                this.CancelButtonText = config.CancelText;
            }
            if (config.SelectedTime != null)
            {
                var time = config.SelectedTime.Value;
                var now = DateTime.Now;
                var date = new DateTime(now.Year, now.Month, now.Day, time.Hours, time.Minutes, 0);
                this.DatePicker.SetDate((NSDate)date, false);
            }
        }
    }
}

