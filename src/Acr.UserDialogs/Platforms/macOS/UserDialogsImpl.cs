using System;
using Acr.UserDialogs.Infrastructure;
using AppKit;
using CoreGraphics;
using Foundation;

namespace Acr.UserDialogs
{
    public class UserDialogsImpl : AbstractUserDialogs
    {
        readonly Func<NSWindow> windowFunc;

        public UserDialogsImpl() : this(() => NSApplication.SharedApplication.MainWindow)
        {
        }

        public UserDialogsImpl(Func<NSWindow> windowFunc)
        {
            this.windowFunc = windowFunc;
        }

        public override IDisposable Alert(AlertConfig config) => this.Present(() =>
        {
            var alert = new NSAlert
            {
                AlertStyle = NSAlertStyle.Informational,
                MessageText = config.Title ?? string.Empty,
                InformativeText = config.Message
            };
            alert.AddButton(config.OkText);
            alert.BeginSheetForResponse(this.windowFunc(), _ => config.OnAction?.Invoke());
            return alert;
        });

        public override IDisposable ActionSheet(ActionSheetConfig config) => this.Present(() => this.CreateNativeActionSheet(config));


        public override IDisposable Confirm(ConfirmConfig config) => this.Present(() =>
        {
            var alert = new NSAlert
            {
                AlertStyle = NSAlertStyle.Informational,
                MessageText = config.Title ?? string.Empty,
                InformativeText = config.Message
            };
            alert.AddButton(config.OkText);
            alert.AddButton(config.CancelText);

            alert.BeginSheetForResponse(this.windowFunc(), result => config.OnAction?.Invoke(result == 1000));
            return alert;
        });

        public override IDisposable DatePrompt(DatePromptConfig config)
        {
            var app = NSApplication.SharedApplication;
            var controller = new DatePickerController(app.MainWindow?.ContentViewController)
            {
                ElementFlags = NSDatePickerElementFlags.YearMonthDate,
                SelectedDateTime = config.SelectedDate ?? DateTime.Now,
                OkText = config.OkText,
                CancelText = config.CancelText,
                Ok = x => config.OnAction?.Invoke(new DatePromptResult(true, x.SelectedDateTime)),
                Cancel = x => config.OnAction?.Invoke(new DatePromptResult(false, x.SelectedDateTime)),
                MinimumDateTime = config.MinimumDate,
                MaximumDateTime = config.MaximumDate
            };

            app.InvokeOnMainThread(() => app.MainWindow?.ContentViewController.PresentViewControllerAsSheet(controller));
            return new DisposableAction(() => app.InvokeOnMainThread(() => app.MainWindow?.ContentViewController.DismissViewController(controller)));
        }


        public override IDisposable TimePrompt(TimePromptConfig config)
        {
            var app = NSApplication.SharedApplication;
            var controller = new DatePickerController(app.MainWindow?.ContentViewController)
            {
                ElementFlags = NSDatePickerElementFlags.HourMinute,
                SelectedDateTime = config.SelectedTime != null ? DateTime.Today.Add((TimeSpan)config.SelectedTime) : DateTime.Now,
                MinuteInterval = config.MinuteInterval,
                OkText = config.OkText,
                CancelText = config.CancelText,
                Ok = x => config.OnAction?.Invoke(new TimePromptResult(true, x.SelectedDateTime.TimeOfDay)),
                Cancel = x => config.OnAction?.Invoke(new TimePromptResult(false, x.SelectedDateTime.TimeOfDay)),
                Use24HourClock = config.Use24HourClock ?? false
            };

            app.InvokeOnMainThread(() => app.MainWindow?.ContentViewController.PresentViewControllerAsSheet(controller));
            return new DisposableAction(() => app.InvokeOnMainThread(() => app.MainWindow?.ContentViewController.DismissViewController(controller)));
        }


        public override IDisposable Login(LoginConfig config) => this.Present(() =>
        {
            var alert = new NSAlert
            {
                AlertStyle = NSAlertStyle.Informational,
                MessageText = config.Title ?? string.Empty,
                InformativeText = config.Message ?? string.Empty
            };
            alert.AddButton(config.OkText);
            alert.AddButton(config.CancelText);

            var inputView = new NSStackView(new CGRect(0, 2, 200, 58));
            var txtUser = new NSTextField(new CGRect(0, 28, 200, 24))
            {
                PlaceholderString = config.LoginPlaceholder,
                StringValue = config.LoginValue ?? string.Empty
            };
            var txtPassword = new NSSecureTextField(new CGRect(0, 2, 200, 24))
            {
                PlaceholderString = config.PasswordPlaceholder
            };

            inputView.AddSubview(txtUser);
            inputView.AddSubview(txtPassword);

            alert.AccessoryView = inputView;
            alert.Layout();

            alert.BeginSheetForResponse(this.windowFunc(), result => config.OnAction?.Invoke(new LoginResult(result == 1000, txtUser.StringValue, txtPassword.StringValue)));
            return alert;
        });


        public override IDisposable Prompt(PromptConfig config) => this.Present(() =>
        {
            var alert = new NSAlert
            {
                AlertStyle = NSAlertStyle.Informational,
                MessageText = config.Title ?? string.Empty,
                InformativeText = config.Message ?? string.Empty
            };
            alert.AddButton(config.OkText);
            if (config.IsCancellable)
                alert.AddButton(config.CancelText);

            var txtInput = new NSTextField(new CGRect(0, 0, 300, 24))
            {
                PlaceholderString = config.Placeholder ?? string.Empty,
                StringValue = config.Text ?? String.Empty
            };

            // TODO: Implement input types validation
            if (config.InputType != InputType.Default || config.MaxLength != null)
                Log.Warn("Acr.UserDialogs", "There is no validation of input types nor MaxLength on this implementation");

            alert.AccessoryView = txtInput;
            alert.Layout();

            alert.BeginSheetForResponse(this.windowFunc(), result => config.OnAction?.Invoke(new PromptResult(result == 1000, txtInput.StringValue)));
            return alert;
        });

        public override IDisposable Toast(ToastConfig cfg)
        {
            // TODO: Implement toast, for now it just shows an alert. 
            return this.Alert(cfg.Message, null, "OK");
        }


        #region Internals

        protected virtual NSAlert CreateNativeActionSheet(ActionSheetConfig config)
        {
            var alert = new NSAlert
            {
                AlertStyle = NSAlertStyle.Informational,
                MessageText = config.Title ?? string.Empty,
                InformativeText = config.Message ?? string.Empty
            };

            if(config.Cancel != null)
                alert.AddButton(config.Cancel.Text);

            var extraSpace = config.Destructive == null ? 30 : 60;
            var inputView = new NSStackView(new CGRect(0, 0, 200, (config.Options.Count * 30) + extraSpace));

            var yPos = config.Options.Count * 30 + (config.Destructive == null ? 0 : 1) * 30;
            foreach (var item in config.Options)
            {
                this.AddActionSheetOption(item, alert.Window, inputView, yPos);
                yPos -= 30;
            }

            if (config.Destructive != null)
                this.AddActionSheetOption(config.Destructive, alert.Window, inputView, yPos, true);

            alert.AccessoryView = inputView;
            alert.Layout();

            alert.BeginSheetForResponse(this.windowFunc(), _ => { });
            return alert;
        }

        protected virtual void AddActionSheetOption(ActionSheetOption opt, NSWindow alertWindow, NSStackView containerView, int yPos, bool isDestructive = false)
        {
            var btn = new NSButton(new CGRect(0, yPos, 200, 28))
            {
                Title = opt.Text ?? string.Empty
            };

            if (isDestructive)
            {
                var colorTitle = new NSMutableAttributedString(btn.AttributedTitle);
                colorTitle.AddAttribute(NSStringAttributeKey.ForegroundColor, NSColor.Red, new NSRange(0, opt.Text.Length));
                btn.AttributedTitle = colorTitle;
            }

            if (opt.ItemIcon != null)
                btn.Image = NSImage.ImageNamed(opt.ItemIcon);

            btn.Activated += (sender, e) =>
            {
                this.windowFunc().EndSheet(alertWindow);
                opt.Action?.Invoke();
            };

            containerView.AddSubview(btn);
        }


        protected override IProgressDialog CreateDialogInstance(ProgressDialogConfig config) => new ProgressDialog(config);

        protected virtual IDisposable Present(Func<NSAlert> alertFunc)
        {
            NSAlert alert = null;
            var app = NSApplication.SharedApplication;
            app.InvokeOnMainThread(() =>
            {
                alert = alertFunc();
            });
            return new DisposableAction(() => app.BeginInvokeOnMainThread(() =>
            {
                if (alert == null)
                    return;

                this.windowFunc().EndSheet(alert.Window);
                alert.Dispose();
            }));
        }

        protected virtual IDisposable Present(NSPanel panel)
        {
            var app = NSApplication.SharedApplication;

            app.InvokeOnMainThread(() => app.MainWindow?.BeginSheet(panel, _ => { }));
            return new DisposableAction(() => app.BeginInvokeOnMainThread(() => app.MainWindow?.EndSheet(panel)));
        }

        #endregion
    }
}
