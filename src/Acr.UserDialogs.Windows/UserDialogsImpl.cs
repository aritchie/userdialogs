using System;
using Windows.UI.Popups;


namespace Acr.UserDialogs.Windows {

    public class UserDialogsImpl : AbstractUserDialogs {

        public override void Alert(AlertConfig config) {
            var dlg = config.Title == null
                ? new MessageDialog(config.Message)
                : new MessageDialog(config.Message, config.Title);

            dlg.ShowAsync();
        }


        public override void ActionSheet(ActionSheetConfig config) {
            throw new NotImplementedException();
        }


        public override void Confirm(ConfirmConfig config) {
            var dialog = new MessageDialog(config.Message, config.Title);
            dialog.Commands.Add(new UICommand(config.OkText, x => config.OnConfirm(true)));
            dialog.Commands.Add(new UICommand(config.CancelText, x => config.OnConfirm(false)));
            dialog.ShowAsync();
        }


        public override void Login(LoginConfig config) {
            throw new NotImplementedException();
        }


        public override void Prompt(PromptConfig config) {
            throw new NotImplementedException();
        }


        public override void Toast(string message, int timeoutSeconds, Action onClick, MaskType maskType) {
            throw new NotImplementedException();
        }


        protected override IProgressDialog CreateDialogInstance() {
            throw new NotImplementedException();
        }
    }
}
