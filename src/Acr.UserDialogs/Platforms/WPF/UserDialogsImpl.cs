using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Acr.UserDialogs
{
    public class UserDialogsImpl : AbstractUserDialogs
    {
        public override IDisposable ActionSheet(ActionSheetConfig config)
        {
            throw new NotImplementedException();
        }

        public override IDisposable Alert(AlertConfig config)
        {
            throw new NotImplementedException();
        }

        public override Task AlertAsync(string message, string title = null, string okText = null,
            CancellationToken? cancelToken = null)
        {
            MessageBox.Show(message, title);
            return Task.FromResult(true);
        }

        public override IDisposable Confirm(ConfirmConfig config)
        {
            throw new NotImplementedException();
        }

        public override IDisposable DatePrompt(DatePromptConfig config)
        {
            throw new NotImplementedException();
        }

        public override IDisposable Login(LoginConfig config)
        {
            throw new NotImplementedException();
        }

        public override IDisposable Prompt(PromptConfig config)
        {
            throw new NotImplementedException();
        }

        public override IDisposable TimePrompt(TimePromptConfig config)
        {
            throw new NotImplementedException();
        }

        public override IDisposable Toast(string title, TimeSpan? dismissTimer = null)
        {
            MessageBox.Show(title);
            return null;
        }

        public override IDisposable Toast(ToastConfig config)
        {
            throw new NotImplementedException();
        }

        protected override IProgressDialog CreateDialogInstance(ProgressDialogConfig config)
        {
            throw new NotImplementedException();
        }
    }
}