using System;
using System.Drawing;
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
            var dlg = new ActionSheetContentDialog();

            var vm = new ActionSheetViewModel {
                Title = config.Title,
                Cancel = new ActionSheetOptionViewModel(config.Cancel != null,config.Cancel?.Text, () => {
                    dlg.Hide();
                    config.Cancel?.Action?.Invoke();
                }),

                Destructive = new ActionSheetOptionViewModel(config.Destructive != null, config.Destructive?.Text, () => {
                    dlg.Hide();
                    config.Destructive?.Action?.Invoke();
                }),

                Options = config
                    .Options
                    .Select(x => new ActionSheetOptionViewModel(true, x.Text, () => {
                        dlg.Hide();
                        x.Action?.Invoke();
                    }))
                    .ToList()
            };

            dlg.DataContext = vm;
            dlg.ShowAsync();
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
            var dlg = new LoginContentDialog();
            var vm = new LoginViewModel {
                LoginText = config.LoginValue,
                Title = config.Title,
                Message = config.Message,
                UserName = config.LoginValue,
                UserNamePlaceholder = config.LoginPlaceholder,
                PasswordPlaceholder = config.PasswordPlaceholder,
                CancelText = config.CancelText
            };
            vm.Login = new Command(() =>
                config.OnResult?.Invoke(new LoginResult(vm.UserName, vm.Password, true))
            );
            vm.Cancel = new Command(() =>
                config.OnResult?.Invoke(new LoginResult(vm.UserName, vm.Password, false))
            );
            dlg.DataContext = vm;
            dlg.ShowAsync();
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
            dialog.PrimaryButtonCommand = new Command(() => {
                config.OnResult?.Invoke(new PromptResult {
                    Ok = true,
                    Text = txt.Text.Trim()
                });
                dialog.Hide();
            });

            if (config.IsCancellable) {
                dialog.SecondaryButtonText = config.CancelText;
                dialog.SecondaryButtonCommand = new Command(() => {
                    config.OnResult?.Invoke(new PromptResult {
                        Ok = false,
                        Text = txt.Text.Trim()
                    });
                    dialog.Hide();
                });
            }
            dialog.ShowAsync();
        }


        public override void ShowImage(IBitmap image, string message, int timeoutMillis) {
        }


        public override void ShowError(string message, int timeoutMillis) {
            this.Show(null, message, ToastConfig.ErrorBackgroundColor, timeoutMillis);
        }


        public override void ShowSuccess(string message, int timeoutMillis) {
            this.Show(null, message, ToastConfig.SuccessBackgroundColor, timeoutMillis);
        }


        void Show(IBitmap image, string message, Color bgColor, int timeoutMillis) {
            var cd = new ContentDialog {
                Background = new SolidColorBrush(bgColor.ToNative()),
                Content = new TextBlock { Text = message }
            };
            cd.ShowAsync();
            Task.Delay(TimeSpan.FromMilliseconds(timeoutMillis)).ContinueWith(x => cd.Hide());
        }


        protected override IProgressDialog CreateDialogInstance() {
            return new ProgressDialog();
        }


        public override void Toast(ToastConfig config) {
            // TODO: action text and action command will work here!
            var stack = new StackPanel { Orientation = Orientation.Horizontal };
            if (config.Icon != null) {
                var icon = config.Icon.ToNative();
                stack.Children.Add(new Image { Source = icon });
            }
            stack.Children.Add(new TextBlock {
                Text = config.Text,
                Foreground = new SolidColorBrush(config.TextColor.ToNative())
            });

            var dialog = new ContentDialog {
                Content = stack,
                Background = new SolidColorBrush(config.BackgroundColor.ToNative())
            };
            dialog.Tapped += (sender, args) => {
                dialog.Hide();
                config.Action?.Invoke();
            };
            dialog.ShowAsync();
            Task.Delay(config.Duration)
                .ContinueWith(x => dialog.Hide());
        }
    }
}
