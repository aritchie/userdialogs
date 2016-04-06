using System;


namespace Acr.UserDialogs
{
    public class DateTimePromptConfig
    {
        public DateTimePromptConfig()
        {
            this.OkText = DefaultOkText;
            this.CancelText = DefaultCancelText;
            this.MinuteInterval = DefaultMinuteInterval;
        }


        public static string DefaultOkText { get; set; } = "Ok";
        public static string DefaultCancelText { get; set; } = "Cancel";
        public static int DefaultMinuteInterval { get; set; } = 1;

        public DateTimePromptMode Mode { get; set; } = DateTimePromptMode.DateAndTime;

        public string Title { get; set; }
        public string OkText { get; set; }
        public string CancelText { get; set; }
        public DateTime? SelectedDateTime { get; set; }

        public Action<DateTimePromptResult> OnResult { get; set; }
        public bool IsCancellable { get; set; } = true;

        public int? MinimumMinutesTimeOfDay { get; set; }
        public int? MaximumMinutesTimeOfDay { get; set; }
        public int MinuteInterval { get; set; }

        public DateTime? MinimumDate { get; set; }
        public DateTime? MaximumDate { get; set; }
    }
}
