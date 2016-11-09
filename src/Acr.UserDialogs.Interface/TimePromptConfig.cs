using System;


namespace Acr.UserDialogs
{
    public class TimePromptConfig
    {
        public static DialogButton DefaultPositive { get; } = new DialogButton(DialogChoice.Positive, "Ok", null, true);
        public static DialogButton DefaultNeutral { get; } = new DialogButton(DialogChoice.Neutral, "Cancel", null, true);
        //public static DialogButton DefaultNegative { get; } = new DialogButton(DialogChoice.Negative, "Remove", null, false);
        public static int DefaultMinuteInterval { get; set; } = 1;
        public static bool? DefaultUse24HourClock { get; set; }

        public string Title { get; set; }
        public bool? Use24HourClock { get; set; } = DefaultUse24HourClock;
        public TimeSpan? SelectedTime { get; set; }
        public DialogButton Positive { get; }  = new DialogButton(DialogChoice.Negative, DefaultPositive.Text, DefaultPositive.TextColor, DefaultPositive.IsVisible);
        public DialogButton Neutral { get; } = new DialogButton(DialogChoice.Neutral, DefaultNeutral.Text, DefaultNeutral.TextColor, DefaultNeutral.IsVisible);
        //public DialogButton Negative { get; } = new DialogButton(DialogChoice.Negative, DefaultNegative.Text, DefaultNegative.TextColor, DefaultNegative.IsVisible);


        public Action<DialogResult<TimeSpan>> OnAction { get; set; }

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
    }
}
