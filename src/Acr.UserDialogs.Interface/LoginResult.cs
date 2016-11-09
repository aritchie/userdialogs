using System;


namespace Acr.UserDialogs
{

    public class LoginResult : AbstractStandardDialogResult<Credentials>
    {
        public string LoginText => this.Value.UserName;
        public string Password => this.Value.Password;


        public LoginResult(bool ok, string login, string pass) : base(ok, new Credentials(login, pass))
        {
        }
    }
}
