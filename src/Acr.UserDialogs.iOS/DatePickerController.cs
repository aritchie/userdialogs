using System;
using Foundation;
using UIKit;


namespace Acr.UserDialogs
{
    public class DatePickerController : ModalDateTimePickerViewController
    {
        public DatePickerController(DatePromptConfig config, UIViewController parent) : base(config.Title, parent)
        {
            this.DatePicker = new UIDatePicker
            {
                Mode = UIDatePickerMode.Date
            };
            this.Dismissed += (object sender, bool e) =>
            {
                var date = ((DateTime)this.DatePicker.Date).ToLocalTime();
                config.OnResult?.Invoke(new DatePromptResult(e, date));
            };
            if (config.SelectedDate != null)
                this.DatePicker.Date = this.ToNsDate(config.SelectedDate.Value, config.UnspecifiedDateTimeKindReplacement);

            if (config.IsCancellable)
                this.CancelButtonText = config.CancelText;

            if (config.MaximumDate != null)
                this.DatePicker.MaximumDate = (NSDate)config.MaximumDate.Value;

            if (config.MinimumDate != null)
                this.DatePicker.MinimumDate = (NSDate)config.MinimumDate.Value;
        }


        protected NSDate ToNsDate(DateTime dateTime, DateTimeKind kind)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
                dateTime = DateTime.SpecifyKind(dateTime, kind);

            return (NSDate) dateTime;
        }
    }
}