using System;
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
                
//                config.OnResult?.Invoke(
            };
        }
    }
}