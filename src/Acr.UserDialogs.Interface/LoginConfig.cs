using System;


namespace Acr.UserDialogs
{

    public class LoginConfig : IAndroidStyleDialogConfig
    {
        public static string DefaultTitle { get; set; } = "Login";
        public static string DefaultPositiveText { get; set; } = "Ok";
        public static string DefaultNeutralText { get; set; } = "Cancel";
        public static string DefaultNegativeText { get; set; }
        public static string DefaultLoginPlaceholder { get; set; } = "User Name";
        public static string DefaultPasswordPlaceholder { get; set; } = "Password";
        public static int? DefaultAndroidStyleId { get; set; }

        public string Title { get; set; } = DefaultTitle;
        public string Message { get; set; }
        public string PositiveText { get; set; } = DefaultPositiveText;
        public string NeutralText { get; set; } = DefaultNeutralText;
        public string NegativeText { get; set; } = DefaultNegativeText;
        public string LoginValue { get; set; }
        public string LoginPlaceholder { get; set; } = DefaultLoginPlaceholder;
        public string PasswordPlaceholder { get; set; } = DefaultPasswordPlaceholder;
        public int? AndroidStyleId { get; set; } = DefaultAndroidStyleId;
        public Action<DialogResult<Credentials>> OnAction { get; set; }


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


        public LoginConfig SetText(DialogChoice choice, string text)
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
