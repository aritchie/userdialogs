using System;
using System.Linq;
using Splat;
#if WPF
using Ookii.Dialogs.Wpf;
#else
using Ookii.Dialogs;
#endif


namespace Acr.UserDialogs
{
    public class UserDialogsImpl : AbstractUserDialogs
    {
        public override IDisposable Alert(AlertConfig config)
        {
            var dlg = new TaskDialog
            {
                WindowTitle = config.Title,
                Content = config.Message,
                Buttons =
                {
                    new TaskDialogButton(config.OkText)
                }
            };
            dlg.ShowDialog();
            return new DisposableAction(dlg.Dispose);
        }


        public override IDisposable ActionSheet(ActionSheetConfig config)
        {
            var dlg = new TaskDialog
            {
                AllowDialogCancellation = config.Cancel != null,
                WindowTitle = config.Title
            };
            config
                .Options
                .ToList()
                .ForEach(x =>
                    dlg.Buttons.Add(new TaskDialogButton(x.Text)
                ));

            dlg.ButtonClicked += (sender, args) =>
            {
                var action = config.Options.First(x => x.Text.Equals(args.Item.Text));
                action.Action();
            };
            dlg.ShowDialog();
            return new DisposableAction(dlg.Dispose);
        }


        public override IDisposable Confirm(ConfirmConfig config)
        {
            var dlg = new TaskDialog
            {
                WindowTitle = config.Title,
                Content = config.Message,
                Buttons =
                {
                    new TaskDialogButton(config.CancelText)
                    {
                        ButtonType = ButtonType.Cancel
                    },
                    new TaskDialogButton(config.OkText)
                    {
                        ButtonType = ButtonType.Ok
                    }
                }
            };
            dlg.ButtonClicked += (sender, args) =>
            {
                var ok = ((TaskDialogButton)args.Item).ButtonType == ButtonType.Ok;
                config.OnAction(ok);
            };
            return new DisposableAction(dlg.Dispose);
        }


        public override IDisposable DatePrompt(DatePromptConfig config)
        {
            throw new NotImplementedException();
        }


        public override IDisposable TimePrompt(TimePromptConfig config)
        {
            throw new NotImplementedException();
        }


        public override IDisposable Login(LoginConfig config)
        {
            var dlg = new CredentialDialog
            {
                //UserName = config.LoginValue ?? String.Empty,
                WindowTitle = config.Title,
                Content = config.Message,
                ShowSaveCheckBox = false
            };
            //dlg.MainInstruction
            dlg.ShowDialog();

            config.OnAction(new LoginResult(
                true,
                dlg.UserName,
                dlg.Password
            ));
            return new DisposableAction(dlg.Dispose);
        }

        public override IDisposable Prompt(PromptConfig config)
        {
#if WPF
            return null;
#else
            var dlg = new InputDialog();
            return new DisposableAction(dlg.Dispose);
#endif
        }


        public override void ShowImage(IBitmap image, string message, int timeoutMillis)
        {
            throw new NotImplementedException();
        }


        public override void ShowSuccess(string message, int timeoutMillis)
        {
            throw new NotImplementedException();
        }


        public override IDisposable Toast(ToastConfig config)
        {
            throw new NotImplementedException();
        }


        protected override IProgressDialog CreateDialogInstance()
        {
            return new ProgressDialogImpl();
        }


        public override void ShowError(string message, int timeoutMillis)
        {
            throw new NotImplementedException();
        }
    }
}
