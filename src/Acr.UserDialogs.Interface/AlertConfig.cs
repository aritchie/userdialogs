using System;
using System.Drawing;


namespace Acr.UserDialogs
{
    public class AlertConfig : IAndroidStyleDialogConfig
    {
        public static DialogButton DefaultPositive { get; } = new DialogButton(DialogChoice.Positive, "Ok", null, true);
        public static DialogButton DefaultNeutral { get; } = new DialogButton(DialogChoice.Neutral, "Cancel", null, false);
        public static DialogButton DefaultNegative { get; } = new DialogButton(DialogChoice.Negative, "Remove", null, false);
        public static Color? DefaultBackgroundColor { get; set; }
        public static int? DefaultAndroidStyleId { get; set; }
        public Action<DialogChoice> OnAction { get; set; }

        public Color? BackgroundColor { get; set; } = DefaultBackgroundColor;
        public DialogButton Positive { get; } = new DialogButton(DialogChoice.Positive, DefaultPositive.Text, DefaultPositive.TextColor, DefaultPositive.IsVisible);
        public DialogButton Neutral { get; } = new DialogButton(DialogChoice.Neutral, DefaultNeutral.Text, DefaultNeutral.TextColor, DefaultNeutral.IsVisible);
        public DialogButton Negative { get; } = new DialogButton(DialogChoice.Negative, DefaultNegative.Text, DefaultNegative.TextColor, DefaultNegative.IsVisible);
        public string Title { get; set; }
        public string Message { get; set; }
        public int? AndroidStyleId { get; set; } = DefaultAndroidStyleId;


        public AlertConfig SetText(DialogChoice choice, string text = null)
        {
            switch (choice)
            {
                case DialogChoice.Negative:
                    this.Negative.Text = text;
                    this.Negative.IsVisible = true;
                    break;

                case DialogChoice.Neutral:
                    this.Neutral.Text = text;
                    this.Neutral.IsVisible = true;
                    break;

                case DialogChoice.Positive:
                    this.Neutral.Text = text;
                    this.Neutral.IsVisible = true;
                    break;
            }
            return this;            
        }


        public AlertConfig SetAction(Action<DialogChoice> action)
        {
            this.OnAction = action;
            return this;
        }


        public AlertConfig SetTitle(string title)
        {
            this.Title = title;
            return this;
        }


        public AlertConfig SetMessage(string message)
        {
            this.Message = message;
            return this;
        }
    }
}
