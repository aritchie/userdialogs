using System;


namespace Acr.UserDialogs
{
    public class TimePromptResult : AbstractStandardDialogResult<TimeSpan>
    {
        public TimePromptResult(bool ok, TimeSpan selectedTime) : base(ok, selectedTime)
        {
        }

        public TimeSpan SelectedTime => this.Value;
    }
}