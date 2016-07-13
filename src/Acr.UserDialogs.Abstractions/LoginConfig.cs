using System;


namespace Acr.UserDialogs {

    public class LoginConfig {
        public static string DefaultTitle { get; set; } = "Login";
        public static string DefaultOkText { get; set; } = "Ok";
        public static string DefaultCancelText { get; set; } = "Cancel";
        public static string DefaultLoginPlaceholder { get; set; } = "User Name";
        public static string DefaultPasswordPlaceholder { get; set; } = "Password";


        public string Title { get; set; }
        public string Message { get; set; }
        public string OkText { get; set; }
        public string CancelText { get; set; }
        public string LoginValue { get; set; }
        public string LoginPlaceholder { get; set; }
        public string PasswordPlaceholder { get; set; }
        public Action<LoginResult> OnResult { get; set; }


        public LoginConfig() {
            this.Title = DefaultTitle;
            this.OkText = DefaultOkText;
            this.CancelText = DefaultCancelText;
            this.LoginPlaceholder = DefaultLoginPlaceholder;
            this.PasswordPlaceholder = DefaultPasswordPlaceholder;
        }


        public LoginConfig SetTitle(string title) {
            this.Title = title;
            return this;
        }


        public LoginConfig SetMessage(string msg) {
            this.Message = msg;
            return this;
        }


        public LoginConfig SetOkText(string ok) {
            this.OkText = ok;
            return this;
        }


        public LoginConfig SetCancelText(string cancel) {
            this.CancelText = cancel;
            return this;
        }


        public LoginConfig SetLoginValue(string txt) {
            this.LoginValue = txt;
            return this;
        }


        public LoginConfig SetLoginPlaceholder(string txt) {
            this.LoginPlaceholder = txt;
            return this;
        }


        public LoginConfig SetPasswordPlaceholder(string txt) {
            this.PasswordPlaceholder = txt;
            return this;
        }


        public LoginConfig SetAction(Action<LoginResult> onResult) {
            this.OnResult = onResult;
            return this;
        }
    }
}
