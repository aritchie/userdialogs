using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls;


namespace Acr.UserDialogs {

    public class UserDialogsImpl : AbstractUserDialogs {

        public override void Alert(AlertConfig config) {
            //var input = new InputDialog();

            //input
            //    .ShowAsync(title, message, okText)
            //    .ContinueWith(x => {
            //        if (onOk != null)
            //            onOk();
            //    });
        }


        public override void ActionSheet(ActionSheetConfig config) {
            var input = new InputDialog {
                ButtonsPanelOrientation = Orientation.Vertical
            };

            var buttons = config
                .Options
                .Select(x => x.Text)
                .ToArray();

            //input
            //    .ShowAsync(options.Title, null, buttons)
            //    .ContinueWith(x => 
            //        options
            //            .Options
            //            .Single(y => y.Text == x.Result)
            //            .Action() 
        //    );
        }


        public override void Confirm(ConfirmConfig config) {
//            var input = new InputDialog {
//                AcceptButton = okText,
//                CancelButton = cancelText
//            };
//            input
//                .ShowAsync(title, message)
//                .ContinueWith(x => {
//                    // TODO: how to get button click for this scenario?
//                });
        }


        public override void Login(LoginConfig config) {
            throw new NotImplementedException();
        }


        public override void Prompt(PromptConfig config) {
//            var input = new InputDialog {
//                AcceptButton = okText,
//                CancelButton = cancelText,
//                InputText = hint
//            };
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
