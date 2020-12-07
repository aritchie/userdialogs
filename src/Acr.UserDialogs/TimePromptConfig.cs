using System;


namespace Acr.UserDialogs
{
    public class TimePromptConfig : IAndroidStyleDialogConfig, IiOSStyleDialogConfig
    {
        public static string DefaultOkText { get; set; } = "Ok";
        public static string DefaultCancelText { get; set; } = "Cancel";
        public static int DefaultMinuteInterval { get; set; } = 1;
        public static bool? DefaultUse24HourClock { get; set; }
        public static int? DefaultAndroidStyleId { get; set; }
        public static iOSPickerStyle? DefaultiOSTimePickerStyle { get; set; }

        public string Title { get; set; }
        public string OkText { get; set; } = DefaultOkText;
        public string CancelText { get; set; } = DefaultCancelText;
        public bool? Use24HourClock { get; set; } = DefaultUse24HourClock;
        public TimeSpan? SelectedTime { get; set; }
        //public bool UwpCancelOnEscKey { get; set; }
        //public bool UwpSubmitOnEnterKey { get; set; }

        public Action<TimePromptResult> OnAction { get; set; }
        public bool IsCancellable { get; set; } = true;

        /// <summary>
        /// Only valid on iOS
        /// </summary>
        public int? MinimumMinutesTimeOfDay { get; set; }

        /// <summary>
        /// Only valid on iOS
        /// </summary>
        public int? MaximumMinutesTimeOfDay { get; set; }

        /// <summary>
        /// Only valid on iOS
        /// </summary>
        public int MinuteInterval { get; set; } = DefaultMinuteInterval;

        public int? AndroidStyleId { get; set; } = DefaultAndroidStyleId;
        public iOSPickerStyle? iOSPickerStyle { get; set; } = DefaultiOSTimePickerStyle;
    }
}
