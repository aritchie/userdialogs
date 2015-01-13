using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls;


namespace Acr.UserDialogs {

    public class UserDialogsImpl : AbstractUserDialogs {

        public override async void Alert(AlertConfig config) {
            var input = new InputDialog();
            await input.ShowAsync(config.Title, config.Message, config.OkText);
            if (config.OnOk != null)
                config.OnOk();
        }


        public override async void ActionSheet(ActionSheetConfig config) {
            var input = new InputDialog {
                ButtonsPanelOrientation = Orientation.Vertical
            };

            var buttons = config
                .Options
                .Select(x => x.Text)
                .ToArray();

            var choice = await input.ShowAsync(config.Title, null, buttons);
            var opt = config.Options.SingleOrDefault(x => x.Text == choice);
            if (opt != null && opt.Action != null)
                opt.Action();
        }


        public override async void Confirm(ConfirmConfig config) {
            var input = new InputDialog {
                AcceptButton = config.OkText,
                CancelButton = config.CancelText
            };
            var choice = await input.ShowAsync(config.Title, config.Message);
            config.OnConfirm(config.OkText == choice);
        }


        public override void Login(LoginConfig config) {
            throw new NotImplementedException();
        }


        public override async void Prompt(PromptConfig config) {
            var input = new InputDialog {
                AcceptButton = config.OkText,
                CancelButton = config.CancelText,
                InputText = config.Placeholder
            };
            var result = await input.ShowAsync(config.Title, config.Message);
//            input
//                .ShowAsync(title, message)
//                .ContinueWith(x => {
//                    // TODO: how to get button click for this scenario?
//                });
        }


        public override void Toast(string message, int timeoutSeconds = 3, Action onClick = null) {
//            //http://msdn.microsoft.com/en-us/library/windows/apps/hh465391.aspx
//            //  TODO: Windows.UI.Notifications.

//            //var toast = new ToastPrompt {
//            //    Message = message,
//            //    MillisecondsUntilHidden = timeoutSeconds * 1000
//            //};
//            //if (onClick != null) {
//            //    toast.Tap += (sender, args) => onClick();
//            //}
//            //toast.Show();
        }


        protected override IProgressDialog CreateDialogInstance() {
            throw new NotImplementedException();
        }
    }
}
