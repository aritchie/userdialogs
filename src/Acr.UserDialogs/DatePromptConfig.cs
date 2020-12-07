using System;


namespace Acr.UserDialogs
{
    public class DatePromptConfig : IAndroidStyleDialogConfig, IiOSStyleDialogConfig
    {
        public static string DefaultOkText { get; set; } = "Ok";
        public static string DefaultCancelText { get; set; } = "Cancel";
        public static DateTimeKind DefaultUnspecifiedDateTimeKindReplacement { get; set; } = DateTimeKind.Local;
        public static int? DefaultAndroidStyleId { get; set; }
        public static iOSPickerStyle? DefaultiOSDatePickerStyle { get; set; }

        public string Title { get; set; }
        public string OkText { get; set; } = DefaultOkText;
        public string CancelText { get; set; } = DefaultCancelText;
        public DateTime? SelectedDate { get; set; }
        public DateTimeKind UnspecifiedDateTimeKindReplacement { get; set; } = DefaultUnspecifiedDateTimeKindReplacement;

        public Action<DatePromptResult> OnAction { get; set; }
        public bool IsCancellable { get; set; } = true;

        public DateTime? MinimumDate { get; set; }
        public DateTime? MaximumDate { get; set; }
        public int? AndroidStyleId { get; set; } = DefaultAndroidStyleId;
        public iOSPickerStyle? iOSPickerStyle { get; set; } = DefaultiOSDatePickerStyle;
    }
}
