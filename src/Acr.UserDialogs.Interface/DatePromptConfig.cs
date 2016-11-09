using System;


namespace Acr.UserDialogs
{
    public class DatePromptConfig
    {
        public static DialogButton DefaultPositive { get; } = new DialogButton(DialogChoice.Positive, "Ok", null, true);
        public static DialogButton DefaultNeutral { get; } = new DialogButton(DialogChoice.Neutral, "Cancel", null, true);
        //public static DialogButton DefaultNegative { get; } = new DialogButton(DialogChoice.Negative, "Remove", null, false);
        public static DateTimeKind DefaultUnspecifiedDateTimeKindReplacement { get; set; } = DateTimeKind.Local;


        public string Title { get; set; }
        public DialogButton Positive { get; }  = new DialogButton(DialogChoice.Negative, DefaultPositive.Text, DefaultPositive.TextColor, DefaultPositive.IsVisible);
        public DialogButton Neutral { get; } = new DialogButton(DialogChoice.Neutral, DefaultNeutral.Text, DefaultNeutral.TextColor, DefaultNeutral.IsVisible);
        //public DialogButton Negative { get; } = new DialogButton(DialogChoice.Negative, DefaultNegative.Text, DefaultNegative.TextColor, DefaultNegative.IsVisible);

        public DateTime? SelectedDate { get; set; }
        public DateTimeKind UnspecifiedDateTimeKindReplacement { get; set; } = DefaultUnspecifiedDateTimeKindReplacement;

        public Action<DialogResult<DateTime>> OnAction { get; set; }
        public DateTime? MinimumDate { get; set; }
        public DateTime? MaximumDate { get; set; }
    }
}
