using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Coding4Fun.Toolkit.Controls;
using Splat;


namespace Acr.UserDialogs {

    public class UserDialogsImpl : AbstractUserDialogs {

        public override void Alert(AlertConfig config) {
            var dialog = new MessageDialog(config.Message, config.Title);
            dialog.Commands.Add(new UICommand(config.OkText, x => config.OnOk?.Invoke()));
            this.Dispatch(() => dialog.ShowAsync());
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
                    }, x.ItemIcon != null ? x.ItemIcon : config.ItemIcon))
                    .ToList()
            };

            dlg.DataContext = vm;
            this.Dispatch(() => dlg.ShowAsync());
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

            var vm = new LoginViewModel {
                LoginText = config.OkText,
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
            var dlg = new LoginContentDialog
            {
                DataContext = vm
            };
            this.Dispatch(() => dlg.ShowAsync());
        }


        public override void Prompt(PromptConfig config)
        {
            var stack = new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = config.Message }
                }
            };
            var dialog = new ContentDialog
            {
                Title = config.Title,
                Content = stack,
                PrimaryButtonText = config.OkText
            };


            if (config.InputType == InputType.Password)
                this.SetPasswordPrompt(dialog, stack, config);
            else
                this.SetDefaultPrompt(dialog, stack, config);

            if (config.IsCancellable) {
                dialog.SecondaryButtonText = config.CancelText;
                dialog.SecondaryButtonCommand = new Command(() =>
                {
                    config.OnResult?.Invoke(new PromptResult { Ok = false });
                    dialog.Hide();
                });
            }

            this.Dispatch(() => dialog.ShowAsync());
        }


        void SetPasswordPrompt(ContentDialog dialog, StackPanel stack, PromptConfig config)
        {
            var txt = new PasswordBox
            {
                PlaceholderText = config.Placeholder,
                Password = config.Text ?? String.Empty
            };
            stack.Children.Add(txt);

            dialog.PrimaryButtonCommand = new Command(() =>
            {
                config.OnResult?.Invoke(new PromptResult
                {
                    Ok = true,
                    Text = txt.Password
                });
                dialog.Hide();
            });
        }


        void SetDefaultPrompt(ContentDialog dialog, StackPanel stack, PromptConfig config)
        {
            var txt = new TextBox
            {
                PlaceholderText = config.Placeholder,
                Text = config.Text ?? String.Empty
            };
            stack.Children.Add(txt);

            dialog.PrimaryButtonCommand = new Command(() =>
            {
                config.OnResult?.Invoke(new PromptResult
                {
                    Ok = true,
                    Text = txt.Text.Trim()
                });
                dialog.Hide();
            });
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
            var cd = new ContentDialog {
                Background = new SolidColorBrush(bgColor.ToNative()),
                Content = new TextBlock { Text = message }
            };
            this.Dispatch(() => cd.ShowAsync());
            Task.Delay(TimeSpan.FromMilliseconds(timeoutMillis))
                .ContinueWith(x => {
                    try {
                        this.Dispatch(() => cd.Hide());
                    }
                    catch { }
                });
        }


        protected override IProgressDialog CreateDialogInstance() {
            return new ProgressDialog();
        }


        public override void Toast(ToastConfig config) {
            var toast = new ToastPrompt {
                Background = new SolidColorBrush(config.BackgroundColor.ToNative()),
                Foreground = new SolidColorBrush(config.TextColor.ToNative()),
                Title = config.Title,
                Message = config.Description,
                ImageSource = config.Icon?.ToNative(),
                Stretch = Stretch.Fill,
                MillisecondsUntilHidden = Convert.ToInt32(config.Duration.TotalMilliseconds)
            };
            //toast.Completed += (sender, args) => {
            //    if (args.PopUpResult == PopUpResult.Ok)
            //        config.Action?.Invoke();
            //};
            this.Dispatch(toast.Show);
        }


        protected virtual void Dispatch(Action action) {
            CoreWindow
                .GetForCurrentThread()
                .Dispatcher
                .RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}