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
                if (config.OnOk != null)
                    alert.Dismissed += (sender, args) => config.OnOk();

                alert.Show();
            });
        }


        public override void ActionSheet(ActionSheetConfig config) {
            var sheet = new CustomMessageBox {
                Caption = config.Title,
                IsLeftButtonEnabled = false,
                IsRightButtonEnabled = false
            };
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
                if (action != null && action.Action != null)
                    action.Action();
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
                PlaceholderText = config.LoginPlaceholder,
                Text = config.LoginValue ?? String.Empty
            };
            var txtPass = new PhonePasswordBox {
                PlaceholderText = config.PasswordPlaceholder
            };
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
                LeftButtonContent = config.OkText,
                RightButtonContent = config.CancelText
            };

            var password = new PasswordBox();
            var inputScope = this.GetInputScope(config.InputType);
            var txt = new PhoneTextBox {
                PlaceholderText = config.Placeholder,
                InputScope = inputScope
            };
            var isSecure = config.InputType == InputType.Password;
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


        public override void Toast(string message, int timeoutSeconds = 3, Action onClick = null) {
            var resources = Application.Current.Resources;

            var tb = new TextBlock {
                Foreground = (Brush)resources["PhoneForegroundBrush"],
                FontSize = (double)resources["PhoneFontSizeMedium"],
                Margin = new Thickness(24, 32, 24, 12),
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = message
            };
            var wrapper = new StackPanel {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = (Brush)resources["PhoneAccentBrush"],
                Width = Application.Current.Host.Content.ActualWidth
            };
            wrapper.Children.Add(tb);

            var popup = new Popup {
                Child = wrapper,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            if (onClick != null) {
                tb.Tap += (sender, args) => {
                    SystemTray.BackgroundColor = (Color)resources["PhoneBackgroundColor"];
                    popup.IsOpen = false;
                    onClick();
                };
            }

            this.Dispatch(() => {
                SystemTray.BackgroundColor = (Color)resources["PhoneAccentColor"];
                popup.IsOpen = true;
            });
            Task.Delay(TimeSpan.FromSeconds(timeoutSeconds))
                .ContinueWith(x => this.Dispatch(() => {
                    SystemTray.BackgroundColor = (Color)resources["PhoneBackgroundColor"];
                    popup.IsOpen = false;
                }));
        }


        protected override IProgressDialog CreateDialogInstance() {
            return new ProgressDialog();
        }


        protected override IProgressIndicator CreateNetworkIndicator() {
            return new NetworkIndicator();
        }


        protected virtual InputScope GetInputScope(InputType inputType) {
            var name = new InputScopeName();
            var scope = new InputScope();

            switch (inputType) {
                case InputType.Email:
                    name.NameValue = InputScopeNameValue.EmailNameOrAddress;
                    break;

                case InputType.Number:
                    name.NameValue = InputScopeNameValue.Number;
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