using System;


namespace Acr.UserDialogs
{
    public class DatePromptResult
    {
        public DatePromptResult(bool ok, DateTime selectedDate)
        {
            this.Ok = ok;
            this.SelectedDate = selectedDate;
        }


        public bool Ok { get; }
        public DateTime SelectedDate { get; set; }
    }
}
