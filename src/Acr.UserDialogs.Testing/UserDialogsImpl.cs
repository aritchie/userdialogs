using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acr.UserDialogs
{
    public class UserDialogsImpl : AbstractUserDialogs
    {
        public override void Alert(AlertConfig config)
        {
        }


        public override void ActionSheet(ActionSheetConfig config)
        {
            throw new NotImplementedException();
        }

        public override void Confirm(ConfirmConfig config)
        {
            throw new NotImplementedException();
        }

        public override void Prompt(PromptConfig config)
        {
            throw new NotImplementedException();
        }

        public override void ShowError(string message, int timeoutMillis)
        {
            throw new NotImplementedException();
        }

        public override void ShowSuccess(string message, int timeoutMillis)
        {
            throw new NotImplementedException();
        }

        public override void Toast(ToastConfig config)
        {
            throw new NotImplementedException();
        }

        protected override IProgressDialog CreateDialogInstance()
        {
            throw new NotImplementedException();
        }

        public override void ShowImage(IBitmap image, string message, int timeoutMillis)
        {
            throw new NotImplementedException();
        }

        public override void Login(LoginConfig config)
        {
            throw new NotImplementedException();
        }
    }
}
