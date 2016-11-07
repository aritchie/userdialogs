using System;


namespace Acr.UserDialogs
{

    public class AlertConfig : IAndroidStyleDialogConfig
    {
        public static string DefaultPositiveText { get; set; } = "Ok";
        public static string DefaultNeutralText { get; set; } = "Cancel";
        public static string DefaultNegativeText { get; set; }
        public static int? DefaultAndroidStyleId { get; set; }

        public string PositiveText { get; set; } = DefaultPositiveText;
        public string NeutralText { get; set; } = DefaultNeutralText;
        public string NegativeText { get; set; } = DefaultNegativeText;
        public string Title { get; set; }
        public string Message { get; set; }
        public int? AndroidStyleId { get; set; } = DefaultAndroidStyleId;
        public Action<DialogChoice> OnAction { get; set; }


        public AlertConfig SetText(DialogChoice choice, string text)
        {
            switch (choice)
            {
                case DialogChoice.Negative:
                    this.NegativeText = text;
                    break;

                case DialogChoice.Neutral:
                    this.NeutralText = text;
                    break;

                case DialogChoice.Positive:
                    this.PositiveText = text;
                    break;
            }
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


        public AlertConfig SetAction(Action<DialogChoice> action)
        {
            this.OnAction = action;
            return this;
        }
    }
}
