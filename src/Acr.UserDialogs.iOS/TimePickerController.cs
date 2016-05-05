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
            if (config.IsCancellable)
            {
                
            }
        }


//            if (this.Config.IsCancellable)
//            {
//                items.Add(this.CreateButton(this.Config.CancelText, () =>
//                {
//                    var date = this.FromNsDate(this.DatePicker.Date);
//                    var result = new TimePromptResult(false, date.TimeOfDay);
//                    this.DismissViewController(true, () => this.Config.OnResult?.Invoke(result));
//                }));
//            }

//            items.Add(this.CreateButton(this.Config.OkText, () =>
//            {
//                var date = this.FromNsDate(this.DatePicker.Date);
//                var result = new TimePromptResult(false, date.TimeOfDay);
//                this.DismissViewController(true, () => this.Config.OnResult?.Invoke(result));
//            }));


//        protected virtual DateTime FromNsDate(NSDate date)
//        {
//            return ((DateTime) date).ToLocalTime();
//        }
    }
}

