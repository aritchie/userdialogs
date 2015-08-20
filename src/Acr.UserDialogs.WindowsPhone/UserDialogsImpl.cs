using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Splat;


namespace Acr.UserDialogs {

    public class UserDialogsImpl : AbstractUserDialogs {

        public override void Alert(AlertConfig config) {
            this.Dispatch(() => {
                var alert = new CustomMessageBox {
                    Caption = config.Title,
                    Message = config.Message,
                    LeftButtonContent = config.OkText,
                    IsRightButtonEnabled = false
                };
                alert.Dismissed += (sender, args) => config.OnOk?.Invoke();

                alert.Show();
            });
        }


        public override void ActionSheet(ActionSheetConfig config) {
            var sheet = new CustomMessageBox {
                Caption = config.Title
            };
            if (config.Cancel != null) {
                sheet.IsRightButtonEnabled = true;
                sheet.RightButtonContent = this.CreateButton(config.Cancel.Text, () => {
                    sheet.Dismiss();
                    config.Cancel.Action?.Invoke();
                });
            }
            if (config.Destructive != null) {
                sheet.IsLeftButtonEnabled = true;
                sheet.LeftButtonContent = this.CreateButton(config.Destructive.Text, () => {
                    sheet.Dismiss();
                    config.Destructive.Action?.Invoke();
                });
            }

            var list = new ListBox {
                FontSize = 36,
                Margin = new Thickness(12.0),
                SelectionMode = SelectionMode.Single,
                ItemsSource = config.Options
                    .Select(x => new TextBlock {
                        Text = x.Text,
                        Margin = new Thickness(0.0, 12.0, 0.0, 12.0),
                        DataContext = x
                    })
            };
            list.SelectionChanged += (sender, args) => sheet.Dismiss();
            sheet.Content = new ScrollViewer {
                Content = list
            };
            sheet.Dismissed += (sender, args) => {
                var txt = list.SelectedValue as TextBlock;
                if (txt == null)
                    return;

                var action = txt.DataContext as ActionSheetOption;
                action?.Action?.Invoke();
            };
            this.Dispatch(sheet.Show);
        }


        public override void Confirm(ConfirmConfig config) {
            var confirm = new CustomMessageBox {
                Caption = config.Title,
                Message = config.Message,
                LeftButtonContent = config.OkText,
                RightButtonContent = config.CancelText
            };
            confirm.Dismissed += (sender, args) => config.OnConfirm(args.Result == CustomMessageBoxResult.LeftButton);
            this.Dispatch(confirm.Show);
        }


        public override void Login(LoginConfig config) {
            var prompt = new CustomMessageBox {
                Caption = config.Title,
                Message = config.Message,
                LeftButtonContent = config.OkText,
                RightButtonContent = config.CancelText
            };
            var txtUser = new PhoneTextBox {
                //PlaceholderText = config.LoginPlaceholder,
                Text = config.LoginValue ?? String.Empty
            };
            var txtPass = new PasswordBox();
            //var txtPass = new PhonePasswordBox {
                //PlaceholderText = config.PasswordPlaceholder
            //};
            var stack = new StackPanel();

            stack.Children.Add(txtUser);
            stack.Children.Add(txtPass);
            prompt.Content = stack;

            prompt.Dismissed += (sender, args) => config.OnResult(new LoginResult(
                txtUser.Text,
                txtPass.Password,
                args.Result == CustomMessageBoxResult.LeftButton
            ));
            this.Dispatch(prompt.Show);
        }


        public override void Prompt(PromptConfig config) {
            var prompt = new CustomMessageBox {
                Caption = config.Title,
                Message = config.Message,
                LeftButtonContent = config.OkText
            };
			if (config.IsCancellable)
				prompt.RightButtonContent = config.CancelText;

            var password = new PasswordBox();
            var inputScope = this.GetInputScope(config.InputType);
            var txt = new PhoneTextBox {
                //PlaceholderText = config.Placeholder,
                InputScope = inputScope
            };
			if (config.Text != null)
				txt.Text = config.Text;

            var isSecure = (config.InputType == InputType.NumericPassword || config.InputType == InputType.Password);
            if (isSecure)
                prompt.Content = password;
            else
                prompt.Content = txt;

            prompt.Dismissed += (sender, args) => config.OnResult(new PromptResult {
                Ok = args.Result == CustomMessageBoxResult.LeftButton,
                Text = isSecure
                    ? password.Password
                    : txt.Text.Trim()
            });
            this.Dispatch(prompt.Show);
        }


        public override void ShowError(string message, int timeoutSeconds) {
            this.Alert(message, null, null);
        }


        public override void ShowSuccess(string message, int timeoutSeconds) {
            this.Alert(message, null, null);
        }


        public override void ShowImage(IBitmap image, string message, int timeoutMillis) {
            this.Alert(message, null, null);
        }


        public override void Toast(ToastConfig cfg) {
            // TODO: backgroundcolor and image
            var resources = Application.Current.Resources;
            var textColor = new SolidColorBrush(cfg.TextColor.ToNative());
            var bgColor = cfg.BackgroundColor.ToNative();

            var wrapper = new StackPanel {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                //Background = (Brush)resources["PhoneAccentBrush"],
                Background = new SolidColorBrush(bgColor),
                Width = Application.Current.Host.Content.ActualWidth
            };
            wrapper.Children.Add(new TextBlock {
                //Foreground = (Brush)resources["PhoneForegroundBrush"],
                Foreground = textColor,
                FontSize = (double)resources["PhoneFontSizeMedium"],
                Margin = new Thickness(24, 32, 24, 12),
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = cfg.Title
            });

            if (!String.IsNullOrWhiteSpace(cfg.Description)) {
                wrapper.Children.Add(new TextBlock {
                    //Foreground = (Brush)resources["PhoneForegroundBrush"],
                    //FontSize = (double)resources["PhoneFontSizeMedium"],
                    Foreground = textColor,
                    FontSize = (double)resources["PhoneFontSizeSmall"],
                    Margin = new Thickness(24, 32, 24, 12),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Text = cfg.Title
                });
            }

            var popup = new Popup {
                Child = wrapper,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            wrapper.Tap += (sender, args) => {
                SystemTray.BackgroundColor = (Color)resources["PhoneBackgroundColor"];
                popup.IsOpen = false;
                cfg.Action?.Invoke();
            };

            this.Dispatch(() => {
                //SystemTray.BackgroundColor = (Color)resources["PhoneAccentColor"];
                SystemTray.BackgroundColor = bgColor;
                popup.IsOpen = true;
            });
            Task.Delay(cfg.Duration)
                .ContinueWith(x => this.Dispatch(() => {
                    SystemTray.BackgroundColor = (Color)resources["PhoneBackgroundColor"];
                    popup.IsOpen = false;
                }));
        }


        protected override IProgressDialog CreateDialogInstance() {
            return new ProgressDialog();
        }


        protected virtual Button CreateButton(string text, Action action) {
            var btn = new Button { Content = text };
            btn.Click += (sender, args) => action();
            return btn;
        }


        protected virtual InputScope GetInputScope(InputType inputType) {
            var name = new InputScopeName();
            var scope = new InputScope();

            switch (inputType) {
                case InputType.Email:
                    name.NameValue = InputScopeNameValue.EmailNameOrAddress;
                    break;

                case InputType.DecimalNumber:
                    name.NameValue = InputScopeNameValue.Digits;
                    break;

                case InputType.Name:
                    name.NameValue = InputScopeNameValue.PersonalFullName;
                    break;

                case InputType.Number:
                    name.NameValue = InputScopeNameValue.Number;
                    break;

                case InputType.NumericPassword:
                    name.NameValue = InputScopeNameValue.NumericPassword;
                    break;

                case InputType.Phone:
                    name.NameValue = InputScopeNameValue.NameOrPhoneNumber;
                    break;

                case InputType.Url:
                    name.NameValue = InputScopeNameValue.Url;
                    break;

				default:
                    name.NameValue = InputScopeNameValue.Default;
                    break;
            }
            scope.Names.Add(name);
            return scope;
        }


        protected virtual void Dispatch(Action action) {
            Deployment.Current.Dispatcher.BeginInvoke(action);
        }
    }
}