using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Splat;


namespace Acr.UserDialogs {

    public class UserDialogsImpl : AbstractUserDialogs {

        public override void Alert(AlertConfig config) {
            var dialog = new MessageDialog(config.Message, config.Title);
            dialog.Commands.Add(new UICommand(config.OkText, x => config.OnOk?.Invoke()));
            dialog.ShowAsync();
        }


        public override void ActionSheet(ActionSheetConfig config) {
            // TODO: need contentdialog /w listview
            var dialog = new MessageDialog(String.Empty, config.Title);
            config.Options.ToList().ForEach(x => dialog.Commands.Add(new UICommand(x.Text, y => x.Action?.Invoke())));

            if (config.Destructive != null) {
                //dialog.Commands.Add();
                // TODO: color red!
            }
            if (config.Cancel != null) {
                dialog.Commands.Add(new UICommand(config.Cancel.Text, x => config.Cancel.Action?.Invoke()));
                dialog.CancelCommandIndex = (uint)dialog.Commands.Count;
            }
            dialog.ShowAsync();
        }


        public override void Confirm(ConfirmConfig config) {
            var dialog = new MessageDialog(config.Message, config.Title);
            dialog.Commands.Add(new UICommand(config.OkText, x => config.OnConfirm(true)));
            dialog.DefaultCommandIndex = 0;

            dialog.Commands.Add(new UICommand(config.CancelText, x => config.OnConfirm(false)));
            dialog.CancelCommandIndex = 1;
            dialog.ShowAsync();
        }


        public override void Login(LoginConfig config) {
            var dialog = new ContentDialog();
            dialog.Content = new LoginPage();

            dialog.ShowAsync();
        }


        public override void Prompt(PromptConfig config) {
            var dialog = new ContentDialog();
            var txt = new TextBox();
            var stack = new StackPanel {
                Children = {
                    new TextBlock { Text = config.Message }
                }
            };
            dialog.Content = stack;


            dialog.PrimaryButtonText = config.OkText;
            dialog.PrimaryButtonClick += (sender, args) => {
                config.OnResult?.Invoke(new PromptResult {
                    Ok = true,
                    Text = txt.Text.Trim()
                });
                dialog.Hide();
            };

            if (config.IsCancellable) {
                dialog.SecondaryButtonText = config.CancelText;
                //dialog.SecondaryButtonCommand = new Command(() => {
                //    config.OnResult?.Invoke(new PromptResult {
                //        Ok = false,
                //        Text = txt.Text.Trim()
                //    });
                //    dialog.Hide();
                //});
            }
            dialog.ShowAsync();
        }


        public override void ShowImage(IBitmap image, string message, int timeoutMillis) {
            throw new NotImplementedException();
        }


        public override void ShowError(string message, int timeoutMillis) {
            throw new NotImplementedException();
        }


        public override void ShowSuccess(string message, int timeoutMillis) {
            throw new NotImplementedException();
        }


        protected override IProgressDialog CreateDialogInstance() {
            var pb = new ProgressBar();

            return null;
        }


        public override void Toast(ToastConfig config) {
            // TODO: action text and action command will work here!
            var dialog = new ContentDialog {
                Title = config.Text
            };
            //dialog.PrimaryButtonText = config
            dialog.Background = new SolidColorBrush(config.BackgroundColor.ToNative());

            dialog.ShowAsync();
            Task.Delay(config.Duration)
                .ContinueWith(x => dialog.Hide());
        }
    }
}
