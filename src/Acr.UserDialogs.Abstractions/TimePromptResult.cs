using System;


namespace Acr.UserDialogs
{
    public class TimePromptResult
    {
        public TimePromptResult(bool ok, TimeSpan selectedTime)
        {
            this.Ok = ok;
            this.SelectedTime = selectedTime;
        }


        public bool Ok { get; }
        public TimeSpan SelectedTime { get; set; }
    }
}
