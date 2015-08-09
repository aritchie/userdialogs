using System;
using System.Windows.Input;
using PropertyChanged;


namespace Acr.UserDialogs {

    [ImplementPropertyChanged]
    public class LoginViewModel {

        public ICommand Login { get; }
        public ICommand Cancel { get; }
        public string UserNameText { get; set; }
        public string UserNamePlaceholder { get; set; }
        public string PasswordText { get; set; }
        public string PasswordPlaceholder { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CancelText { get; set; }
        public string LoginText { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
