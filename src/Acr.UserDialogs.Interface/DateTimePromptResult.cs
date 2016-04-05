using System;


namespace Acr.UserDialogs
{
    public class DateTimePromptResult
    {
        public DateTimePromptResult(bool ok, DateTime selectedDateTime)
        {
            this.Ok = ok;
            this.SelectedDateTime = selectedDateTime;
        }


        public bool Ok { get; }
        public DateTime SelectedDateTime { get; set; }
    }
}
