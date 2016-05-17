using System;


namespace Acr.UserDialogs
{
    public class DatePromptConfig
    {
        public DatePromptConfig()
        {
            this.OkText = DefaultOkText;
            this.CancelText = DefaultCancelText;
            this.UnspecifiedDateTimeKindReplacement = DefaultUnspecifiedDateTimeKindReplacement;
        }


        public static string DefaultOkText { get; set; } = "Ok";
        public static string DefaultCancelText { get; set; } = "Cancel";
        public static DateTimeKind DefaultUnspecifiedDateTimeKindReplacement { get; set; } = DateTimeKind.Local;


        public string Title { get; set; }
        public string OkText { get; set; }
        public string CancelText { get; set; }
        public DateTime? SelectedDate { get; set; }
        public DateTimeKind UnspecifiedDateTimeKindReplacement { get; set; }

        public Action<DatePromptResult> OnResult { get; set; }
        public bool IsCancellable { get; set; } = true;

        public DateTime? MinimumDate { get; set; }
        public DateTime? MaximumDate { get; set; }
    }
}
