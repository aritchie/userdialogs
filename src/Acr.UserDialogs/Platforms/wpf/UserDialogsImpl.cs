using System;


namespace Acr.UserDialogs
{
    using System.Windows;
    using System.Windows.Controls;

    public class UserDialogsImpl : AbstractUserDialogs
    {

        public override IDisposable Alert(AlertConfig config)
        {
            var result = MessageBox.Show(config.Message, "", MessageBoxButton.OK);
            config.OnAction?.Invoke();
            return null;
        }


        public override IDisposable ActionSheet(ActionSheetConfig config)
        {
            var result = MessageBox.Show(config.Message, "", MessageBoxButton.OK);

            foreach (var opt in config.Options)
            {
                //var btn = alert.AddButton(opt.Text);
                //if (opt.ItemIcon != null)
                //    btn.Image = opt.ItemIcon.ToNative();
            }
            //var actionIndex = alert.RunSheetModal(null); // TODO: get top NSWindow
            //config.Options[(int)actionIndex].Action?.Invoke();
            return null;
        }


        public override IDisposable Confirm(ConfirmConfig config)
        {
            var result = MessageBox.Show(config.Message, "", MessageBoxButton.OKCancel);
            config.OnAction?.Invoke(result == MessageBoxResult.OK);
            return null;
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
            var txt = new TextBox();
            //var alert = new NSAlert
            //{
            //    AccessoryView = txt,
            //    MessageText = config.Message
            //};
            //alert.AddButton(config.OkText);
            //if (config.CancelText != null)
            //    alert.AddButton(config.CancelText);

            //var actionIndex = alert.RunModal();
            //config.OnAction?.Invoke(new LoginResult(actionIndex == 0, ));

            return null;
        }


        public override IDisposable Prompt(PromptConfig config)
        {
            //var txt = new NSTextField(new CGRect(0, 0, 300, 20));
            //var alert = new NSAlert
            //{
            //    AccessoryView = txt,
            //    MessageText = config.Message
            //};
            //alert.AddButton(config.OkText);
            //if (config.CancelText != null)
            //    alert.AddButton(config.CancelText);

            //var actionIndex = alert.RunModal();
            //config.OnAction?.Invoke(new PromptResult(actionIndex == 0, txt.StringValue));

            return null;
        }


        public override IDisposable Toast(ToastConfig config)
        {
            var result = MessageBox.Show(config.Message, "", MessageBoxButton.OK);
            return null;
        }


        protected override IProgressDialog CreateDialogInstance(ProgressDialogConfig config)
        {
            throw new NotImplementedException();
        }
    }
}
