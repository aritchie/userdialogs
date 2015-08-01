using System;
using System.Linq;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;


namespace Acr.UserDialogs {

    public class UserDialogsImpl : AbstractUserDialogs {

        public override void Alert(AlertConfig config) {
            var dlg = config.Title == null
                ? new MessageDialog(config.Message)
                : new MessageDialog(config.Message, config.Title);

            dlg.ShowAsync();
        }


        public override void ActionSheet(ActionSheetConfig config) {
            var sheet = new PopupMenu();
            foreach (var opt in config.Options)
                sheet.Commands.Add(new UICommand(opt.Text, x => opt.Action?.Invoke()));

            if (config.Cancel != null)
                sheet.Commands.Add(new UICommand(config.Cancel.Text, x => config.Cancel.Action?.Invoke()));

            if (config.Destructive != null)
                sheet.Commands.Add(new UICommand(config.Destructive.Text, x => config.Destructive.Action?.Invoke()));

            sheet.ShowAsync(new Point(0, 0));
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


        public override void Toast(ToastConfig cfg) {
            throw new NotImplementedException();
        }


        protected override IProgressDialog CreateDialogInstance() {
            throw new NotImplementedException();
        }
    }
}
