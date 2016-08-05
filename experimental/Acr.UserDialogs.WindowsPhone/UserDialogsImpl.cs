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


namespace Acr.UserDialogs
{

    public class UserDialogsImpl : AbstractUserDialogs
    {

        public override IDisposable Alert(AlertConfig config)
        {
            var alert = new CustomMessageBox
            {
                Caption = config.Title,
                Message = config.Message,
                LeftButtonContent = config.OkText,
                IsRightButtonEnabled = false
            };
            alert.Dismissed += (sender, args) => config.OnAction?.Invoke();
            return this.DispatchWithDispose(alert.Show, alert.Dismiss);
        }


        public override IDisposable ActionSheet(ActionSheetConfig config)
        {
            var sheet = new CustomMessageBox
            {
                Caption = config.Title
            };
            if (config.Cancel != null)
            {
                sheet.IsRightButtonEnabled = true;
                sheet.RightButtonContent = this.CreateButton(config.Cancel.Text, () =>
                {
                    sheet.Dismiss();
                    config.Cancel.Action?.Invoke();
                });
            }
            if (config.Destructive != null)
            {
                sheet.IsLeftButtonEnabled = true;
                sheet.LeftButtonContent = this.CreateButton(config.Destructive.Text, () =>
                {
                    sheet.Dismiss();
                    config.Destructive.Action?.Invoke();
                });
            }

            var list = new ListBox
            {
                FontSize = 36,
                Margin = new Thickness(12.0),
                SelectionMode = SelectionMode.Single,
                ItemsSource = config.Options
                    .Select(x => new TextBlock
                    {
                        Text = x.Text,
                        Margin = new Thickness(0.0, 12.0, 0.0, 12.0),
                        DataContext = x
                    })
            };
            list.SelectionChanged += (sender, args) => sheet.Dismiss();
            sheet.Content = new ScrollViewer
            {
                Content = list
            };
            sheet.Dismissed += (sender, args) =>
            {
                var txt = list.SelectedValue as TextBlock;
                if (txt == null)
                    return;

                var action = txt.DataContext as ActionSheetOption;
                action?.Action?.Invoke();
            };
            return this.DispatchWithDispose(sheet.Show, sheet.Dismiss);
        }


        public override IDisposable Confirm(ConfirmConfig config)
        {
            var confirm = new CustomMessageBox
            {
                Caption = config.Title,
                Message = config.Message,
                LeftButtonContent = config.OkText,
                RightButtonContent = config.CancelText
            };
            confirm.Dismissed += (sender, args) => config.OnAction(args.Result == CustomMessageBoxResult.LeftButton);
            return this.DispatchWithDispose(confirm.Show, confirm.Dismiss);
        }


        public override IDisposable DatePrompt(DatePromptConfig config)
        {
            throw new NotImplementedException();
        }


        public override IDisposable TimePrompt(TimePromptConfig config)
        {
            throw new NotImplementedException();
        }


        public override IDisposable Login(LoginConfig config)
        {
            var prompt = new CustomMessageBox
            {
                Caption = config.Title,
                Message = config.Message,
                LeftButtonContent = config.OkText,
                RightButtonContent = config.CancelText
            };
            var txtUser = new PhoneTextBox
            {
                Text = config.LoginValue ?? String.Empty
            };
            var txtPass = new PasswordBox();
            var stack = new StackPanel();

            stack.Children.Add(txtUser);
            stack.Children.Add(txtPass);
            prompt.Content = stack;

            prompt.Dismissed += (sender, args) => config.OnAction(new LoginResult(
                args.Result == CustomMessageBoxResult.LeftButton,
                txtUser.Text,
                txtPass.Password
            ));
            return this.DispatchWithDispose(prompt.Show, prompt.Dismiss);
        }


        public override IDisposable Prompt(PromptConfig config)
        {
            var prompt = new CustomMessageBox
            {
                Caption = config.Title,
                Message = config.Message,
                LeftButtonContent = config.OkText
            };
            if (config.IsCancellable)
                prompt.RightButtonContent = config.CancelText;

            var password = new PasswordBox();
            var inputScope = this.GetInputScope(config.InputType);
            var txt = new PhoneTextBox
            {
                InputScope = inputScope
            };
            if (config.Text != null)
                txt.Text = config.Text;

            var isSecure = (config.InputType == InputType.NumericPassword || config.InputType == InputType.Password);
            if (isSecure)
                prompt.Content = password;
            else
                prompt.Content = txt;

            prompt.Dismissed += (sender, args) =>
            {
                var ok = args.Result == CustomMessageBoxResult.LeftButton;
                var text = isSecure ? password.Password : txt.Text.Trim();
                config.OnAction(new PromptResult(ok, text));
            };
            return this.DispatchWithDispose(prompt.Show, prompt.Dismiss);
        }


        public override void ShowError(string message, int timeoutSeconds)
        {
            this.Alert(message, null, null);
        }


        public override void ShowSuccess(string message, int timeoutSeconds)
        {
            this.Alert(message, null, null);
        }


        public override void ShowImage(IBitmap image, string message, int timeoutMillis)
        {
            this.Alert(message, null, null);
        }


        public override IDisposable Toast(ToastConfig cfg)
        {
            var resources = Application.Current.Resources;

            var wrapper = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Width = Application.Current.Host.Content.ActualWidth
            };
            if (cfg.BackgroundColor != null)
                wrapper.Background = new SolidColorBrush(cfg.BackgroundColor.Value.ToNative());

            var txt = new TextBlock
            {
                FontSize = (double) resources["PhoneFontSizeMedium"],
                Margin = new Thickness(24, 32, 24, 12),
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = cfg.Message
            };
            if (cfg.MessageTextColor != null)
                txt.Foreground = new SolidColorBrush(cfg.MessageTextColor.Value.ToNative());

            wrapper.Children.Add(txt);
            var popup = new Popup
            {
                Child = wrapper,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            Action close = () =>
            {
                SystemTray.BackgroundColor = (Color)resources["PhoneBackgroundColor"];
                popup.IsOpen = false;
            };

            wrapper.Tap += (sender, args) =>
            {
                close();
                cfg.Action.Action?.Invoke();
            };

            Task.Delay(cfg.Duration)
                .ContinueWith(x => this.Dispatch(close));

            return this.DispatchWithDispose(() =>
            {
                //SystemTray.BackgroundColor = bgColor;
                popup.IsOpen = true;
            }, close);
        }


        protected override IProgressDialog CreateDialogInstance(ProgressDialogConfig config)
        {
            return new ProgressDialog(config);
        }


        protected virtual Button CreateButton(string text, Action action)
        {
            var btn = new Button { Content = text };
            btn.Click += (sender, args) => action();
            return btn;
        }


        protected virtual InputScope GetInputScope(InputType inputType)
        {
            var name = new InputScopeName();
            var scope = new InputScope();

            switch (inputType)
            {
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


        protected virtual void Dispatch(Action action)
        {
            Deployment.Current.Dispatcher.BeginInvoke(action);
        }


        protected virtual IDisposable DispatchWithDispose(Action dispatch, Action dispose)
        {
            this.Dispatch(dispatch);
            return new DisposableAction(() =>
            {
                try
                {
                    this.Dispatch(dispose);
                }
                catch
                {
                }
            });
        }
    }
}