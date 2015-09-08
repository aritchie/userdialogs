using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
//using Coding4Fun.Toolkit.Controls;
using Splat;
using Acr.UserDialogs;

[assembly: Xamarin.Forms.Dependency(typeof(UserDialogsImpl))]

namespace Acr.UserDialogs
{


    public class UserDialogsImpl : AbstractUserDialogs {

        public override void Alert(AlertConfig config) {
            var dialog = new MessageDialog(config.Message, config.Title);
            dialog.Commands.Add(new UICommand(config.OkText, x => config.OnOk?.Invoke()));
            this.Dispatch(() => dialog.ShowAsync());
        }


        public override void ActionSheet(ActionSheetConfig config) {
            throw new NotImplementedException();
        }


        public override void Confirm(ConfirmConfig config) {
            var dialog = new MessageDialog(config.Message, config.Title);
            dialog.Commands.Add(new UICommand(config.OkText, x => config.OnConfirm(true)));
            dialog.DefaultCommandIndex = 0;

            dialog.Commands.Add(new UICommand(config.CancelText, x => config.OnConfirm(false)));
            dialog.CancelCommandIndex = 1;
            this.Dispatch(() => dialog.ShowAsync());
        }


        public override void Login(LoginConfig config) {
            throw new NotImplementedException();
        }


        public override void Prompt(PromptConfig config) {
            throw new NotImplementedException();
        }


        public override void ShowImage(IBitmap image, string message, int timeoutMillis) {
            this.Show(null, message, ToastConfig.SuccessBackgroundColor, timeoutMillis);
        }


        public override void ShowError(string message, int timeoutMillis) {
            this.Show(null, message, ToastConfig.ErrorBackgroundColor, timeoutMillis);
        }


        public override void ShowSuccess(string message, int timeoutMillis) {
            this.Show(null, message, ToastConfig.SuccessBackgroundColor, timeoutMillis);
        }


        void Show(IBitmap image, string message, Color bgColor, int timeoutMillis) {
            throw new NotImplementedException();
        }


        protected override IProgressDialog CreateDialogInstance() {
            throw new NotImplementedException();
        }


        public override void Toast(ToastConfig config) {
            throw new NotImplementedException();
        }


        protected virtual void Dispatch(Action action) {
            CoreWindow
                .GetForCurrentThread()
                .Dispatcher
                .RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}