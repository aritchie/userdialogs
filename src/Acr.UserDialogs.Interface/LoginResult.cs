using System;


namespace Acr.UserDialogs {
    
    public class LoginResult {

        public string LoginText { get; private set; }
        public string Password { get; private set; }
        public bool Ok { get; private set; }


        public LoginResult(string login, string pass, bool ok) {
            this.LoginText = login;
            this.Password = pass;
            this.Ok = ok;
        }
    }
}
