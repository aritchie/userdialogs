using System;
using System.Linq;
using Windows.UI.Notifications;
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
 //var toastXmlString = string.Format("<toast><visual version='1'><binding template='ToastText01'><text id='1'>{0}</text></binding></visual></toast>", message);
 //  var xmlDoc = new Windows.Data.Xml.Dom.XmlDocument();
 //  xmlDoc.LoadXml(toastXmlString);
 //  var toast = new ToastNotification(xmlDoc);
 //  ToastNotificationManager.CreateToastNotifier().Show(toast);
        }


        protected override IProgressDialog CreateDialogInstance() {
            throw new NotImplementedException();
        }


        protected override IProgressIndicator CreateNetworkIndicator() {
            return new NetworkIndicator();
        }
    }
}
