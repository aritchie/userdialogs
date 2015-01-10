using System;
using System.Threading.Tasks;


namespace Acr.UserDialogs {

    public class UserDialogsImpl : AbstractUserDialogs {

        public override void Alert(AlertConfig config) {
            throw new NotImplementedException();
        }


        public override void ActionSheet(ActionSheetConfig config) {
            throw new NotImplementedException();
        }


        public override void Confirm(ConfirmConfig config) {
            throw new NotImplementedException();
        }


        public override void Login(LoginConfig config) {
            throw new NotImplementedException();
        }


        public override void Prompt(PromptConfig config) {
            throw new NotImplementedException();
        }


        public override void Toast(string message, int timeoutSeconds = 3, Action onClick = null) {
            throw new NotImplementedException();
        }


        protected override IProgressDialog CreateDialogInstance() {
            throw new NotImplementedException();
        }
    }
}

//using System;
//using System.Linq;
//using Windows.UI.Xaml.Controls;
//using WinRTXamlToolkit.Controls;


//namespace Acr.MvvmCross.Plugins.UserDialogs.WindowsStore {

//    public class WinStoreUserDialogService : AbstractUserDialogService<WinStoreProgressDialog> {
//        // TODO: dispatching

//        public override void ActionSheet(ActionSheetOptions options) {
//            var input = new InputDialog {
//                ButtonsPanelOrientation = Orientation.Vertical
//            };

//            var buttons = options.Options
//                .Select(x => x.Text)
//                .ToArray();

//            input
//                .ShowAsync(options.Title, null, buttons)
//                .ContinueWith(x => 
//                    options
//                        .Options
//                        .Single(y => y.Text == x.Result)
//                        .Action() 
//                );
//        }


//        public override void Alert(string message, string title, string okText, Action onOk) {
//            var input = new InputDialog();

//            input
//                .ShowAsync(title, message, okText)
//                .ContinueWith(x => {
//                    if (onOk != null)
//                        onOk();
//                });
//        }


//        public override void Confirm(string message, Action<bool> onConfirm, string title, string okText, string cancelText) {
//            var input = new InputDialog {
//                AcceptButton = okText,
//                CancelButton = cancelText
//            };
//            input
//                .ShowAsync(title, message)
//                .ContinueWith(x => {
//                    // TODO: how to get button click for this scenario?
//                });
//        }


//        public override void Prompt(string message, Action<PromptResult> promptResult, string title, string okText, string cancelText, string hint) {
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
//        }


//        public override void Toast(string message, int timeoutSeconds, Action onClick) {
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
//        }


//        protected override WinStoreProgressDialog CreateProgressDialogInstance() {
//            return new WinStoreProgressDialog();
//        }
//    }
//}
