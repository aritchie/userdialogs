using System;
using System.Drawing;


namespace Acr.UserDialogs
{

    public class LoginConfig : IAndroidStyleDialogConfig
    {
        public static string DefaultTitle { get; set; } = "Login";
        public static DialogButton DefaultPositive { get; } = new DialogButton(DialogChoice.Positive, "Ok", null, true);
        public static DialogButton DefaultNeutral { get; } = new DialogButton(DialogChoice.Neutral, "Cancel", null, false);
        public static DialogButton DefaultNegative { get; } = new DialogButton(DialogChoice.Negative, "Remove", null, true);
        public static string DefaultLoginPlaceholder { get; set; } = "User Name";
        public static string DefaultPasswordPlaceholder { get; set; } = "Password";
        public static Color? DefaultBackgroundColor { get; set; }
        public static int? DefaultAndroidStyleId { get; set; }

        public Color? BackgroundColor { get; set; }
        public string Title { get; set; } = DefaultTitle;
        public string Message { get; set; }
        public DialogButton Positive { get; } = new DialogButton(DialogChoice.Positive, DefaultPositive.Text, DefaultPositive.TextColor, DefaultPositive.IsVisible);
        public DialogButton Neutral { get; } = new DialogButton(DialogChoice.Neutral, DefaultNeutral.Text, DefaultNeutral.TextColor, DefaultNeutral.IsVisible);
        public DialogButton Negative { get; } = new DialogButton(DialogChoice.Negative, DefaultNegative.Text, DefaultNegative.TextColor, DefaultNegative.IsVisible);
        public string LoginValue { get; set; }
        public string LoginPlaceholder { get; set; } = DefaultLoginPlaceholder;
        public string PasswordPlaceholder { get; set; } = DefaultPasswordPlaceholder;
        public int? AndroidStyleId { get; set; } = DefaultAndroidStyleId;
        public Action<DialogResult<Credentials>> OnAction { get; set; }


        public LoginConfig SetText(DialogChoice choice, string text = null)
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


        public LoginConfig SetTitle(string title)
        {
            this.Title = title;
            return this;
        }


        public LoginConfig SetMessage(string msg)
        {
            this.Message = msg;
            return this;
        }


        public LoginConfig SetLoginValue(string txt)
        {
            this.LoginValue = txt;
            return this;
        }


        public LoginConfig SetLoginPlaceholder(string txt)
        {
            this.LoginPlaceholder = txt;
            return this;
        }


        public LoginConfig SetPasswordPlaceholder(string txt)
        {
            this.PasswordPlaceholder = txt;
            return this;
        }


        public LoginConfig SetAction(Action<DialogResult<Credentials>> action)
        {
            this.OnAction = action;
            return this;
        }
    }
}
