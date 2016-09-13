using System;
using System.Linq;
using UIKit;
using CoreGraphics;
using Foundation;
using Acr.Support.iOS;
using BigTed;
using Splat;


namespace Acr.UserDialogs
{
    public class UserDialogsImpl : AbstractUserDialogs
    {
        readonly Func<UIViewController> viewControllerFunc;


        public UserDialogsImpl() : this(() => UIApplication.SharedApplication.GetTopViewController())
        {
        }


        public UserDialogsImpl(Func<UIViewController> viewControllerFunc)
        {
            this.viewControllerFunc = viewControllerFunc;
        }


        public override IDisposable Alert(AlertConfig config)
        {
            return this.Present(() =>
            {
                var alert = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnAction?.Invoke()));
                return alert;
            });
        }


        public override IDisposable ActionSheet(ActionSheetConfig config)
        {
            return this.Present(() => this.CreateNativeActionSheet(config));
        }


        public override IDisposable Confirm(ConfirmConfig config)
        {
            return this.Present(() =>
            {
                var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
                dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x => config.OnAction(false)));
                dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnAction(true)));
                return dlg;
            });
        }


        public override IDisposable DatePrompt(DatePromptConfig config)
        {
            var picker = new AI.AIDatePickerController
            {
                Mode = UIDatePickerMode.Date,
                SelectedDateTime = config.SelectedDate ?? DateTime.Now,
                OkText = config.OkText,
                CancelText = config.CancelText,
                Ok = x => config.OnAction (new DatePromptResult (true, x.SelectedDateTime)),
                Cancel = x => config.OnAction(new DatePromptResult(false, x.SelectedDateTime)),
            };
            if (config.MaximumDate != null)
                picker.MaximumDateTime = config.MaximumDate;

            if (config.MinimumDate != null)
                picker.MinimumDateTime = config.MinimumDate;

            return this.Present(picker);
        }


        public override IDisposable TimePrompt(TimePromptConfig config)
        {
            var picker = new AI.AIDatePickerController
            {
                Mode = UIDatePickerMode.Time,
				SelectedDateTime = config.SelectedTime != null ? DateTime.Today.Add ((TimeSpan)config.SelectedTime) : DateTime.Now,
                MinuteInterval = config.MinuteInterval,
                OkText = config.OkText,
                CancelText = config.CancelText,
                Ok = x => config.OnAction(new TimePromptResult(true, x.SelectedDateTime.TimeOfDay)),
                Cancel = x => config.OnAction(new TimePromptResult(false, x.SelectedDateTime.TimeOfDay)),
                Use24HourClock = config.Use24HourClock
            };
            return this.Present(picker);
        }

        public override IDisposable MultiPickerPrompt(MultiPickerPromptConfig config)
        {
            var picker = new AI.AIMultiPickerController(config)
            {
                OkText = config.OkText,
                CancelText = config.CancelText,
                Ok = x => config.OnAction(new MultiPickerPromptResult(true, x.SelectedItems)),
                Cancel = x => config.OnAction(new MultiPickerPromptResult(false, x.SelectedItems)),
            };
            return this.Present(picker);
        }


        public override IDisposable Login(LoginConfig config)
        {

            return this.Present(() =>
            {
                UITextField txtUser = null;
                UITextField txtPass = null;

                var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
                dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x => config.OnAction(new LoginResult(false, txtUser.Text, txtPass.Text))));
                dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnAction(new LoginResult(true, txtUser.Text, txtPass.Text))));
                dlg.AddTextField(x =>
                {
                    txtUser = x;
                    x.Placeholder = config.LoginPlaceholder;
                    x.Text = config.LoginValue ?? String.Empty;
                });
                dlg.AddTextField(x =>
                {
                    txtPass = x;
                    x.Placeholder = config.PasswordPlaceholder;
                    x.SecureTextEntry = true;
                });
                return dlg;
            });
        }


        public override IDisposable Prompt(PromptConfig config)
        {
            return this.Present(() =>
            {
                var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
                UITextField txt = null;

                if (config.IsCancellable)
                {
                    dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x =>
                        config.OnAction(new PromptResult(false, txt.Text.Trim())
                    )));
                }
                dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x =>
                    config.OnAction(new PromptResult(true, txt.Text.Trim())
                )));
                dlg.AddTextField(x =>
                {
                    if (config.MaxLength != null)
                    {
                        txt.ShouldChangeCharacters =  (tf, replace, range) =>
                        {
                            var len = txt.Text.Length + replace.Length - range.Length;
                            return len <= config.MaxLength.Value;
                        };
                    }
                    this.SetInputType(x, config.InputType);
                    x.Placeholder = config.Placeholder ?? String.Empty;
                    if (config.Text != null)
                        x.Text = config.Text;

                    txt = x;
                });
                return dlg;
            });
        }


        public override void ShowImage(IBitmap image, string message, int timeoutMillis)
        {
            BTProgressHUD.ShowImage(image.ToNative(), message, timeoutMillis);
            //this.ShowWithOverlay(timeoutMillis, () => BTProgressHUD.ShowImage(image.ToNative(), message, timeoutMillis));
        }


        public override void ShowError(string message, int timeoutMillis)
        {
            BTProgressHUD.ShowErrorWithStatus(message, timeoutMillis);
            //this.ShowWithOverlay(timeoutMillis, () => BTProgressHUD.ShowErrorWithStatus(message, timeoutMillis));
            //this.ShowWithOverlay(timeoutMillis, () => BTProgressHUD.ShowImage(UIImage.FromBundle("icon-error"), message, timeoutMillis));
        }


        public override void ShowSuccess(string message, int timeoutMillis)
        {

            BTProgressHUD.ShowSuccessWithStatus(message, timeoutMillis);
            //this.ShowWithOverlay(timeoutMillis, () => BTProgressHUD.ShowSuccessWithStatus(message, timeoutMillis));
            //this.ShowWithOverlay(timeoutMillis, () => BTProgressHUD.ShowImage(UIImage.FromBundle("icon-success"), message, timeoutMillis));
        }


        IDisposable currentToast;
        public override IDisposable Toast(ToastConfig cfg)
        {
            this.currentToast?.Dispose();

            var app = UIApplication.SharedApplication;
            app.InvokeOnMainThread(() =>
            {
                var snackbar = new TTG.TTGSnackbar
                {
                    Message = cfg.Message,
                    Duration = cfg.Duration,
                    AnimationType = TTG.TTGSnackbarAnimationType.FadeInFadeOut
                };
                if (cfg.BackgroundColor != null)
                    snackbar.BackgroundColor = cfg.BackgroundColor.Value.ToNative();

                if (cfg.MessageTextColor != null)
                    snackbar.MessageLabel.TextColor = cfg.MessageTextColor.Value.ToNative();

                if (cfg.Action != null)
                {
                    var color = cfg.Action.TextColor ?? ToastConfig.DefaultActionTextColor;
                    if (color != null)
                        snackbar.ActionButton.SetTitleColor(color.Value.ToNative(), UIControlState.Normal);

                    snackbar.ActionText = cfg.Action.Text;
                    snackbar.ActionBlock = x =>
                    {
                        snackbar.Dismiss();
                        cfg.Action.Action?.Invoke();
                    };
                }
                snackbar.Show();

                this.currentToast = new DisposableAction(
                    () => app.InvokeOnMainThread(() => snackbar.Dismiss())
                );
            });
            return this.currentToast;
        }


        #region Internals

        protected virtual UIAlertController CreateNativeActionSheet(ActionSheetConfig config)
        {
            var sheet = UIAlertController.Create(config.Title, null, UIAlertControllerStyle.ActionSheet);

            if (config.Destructive != null)
                this.AddActionSheetOption(config.Destructive, sheet, UIAlertActionStyle.Destructive);

            config
                .Options
                .ToList()
                .ForEach(x => this.AddActionSheetOption(x, sheet, UIAlertActionStyle.Default, config.ItemIcon));

            if (config.Cancel != null)
                this.AddActionSheetOption(config.Cancel, sheet, UIAlertActionStyle.Cancel);

            return sheet;
        }

        protected virtual void AddActionSheetOption(ActionSheetOption opt, UIAlertController controller, UIAlertActionStyle style, IBitmap image = null)
        {
            var alertAction = UIAlertAction.Create(opt.Text, style, x => opt.Action?.Invoke());

            if (opt.ItemIcon == null && image != null)
                opt.ItemIcon = image;

            if (opt.ItemIcon != null)
                alertAction.SetValueForKey(opt.ItemIcon.ToNative(), new NSString("image"));

            controller.AddAction(alertAction);
        }


        protected override IProgressDialog CreateDialogInstance(ProgressDialogConfig config)
        {
            return new ProgressDialog(config);
        }


        protected virtual IDisposable Present(Func<UIAlertController> alertFunc)
        {
            UIAlertController alert = null;
            var app = UIApplication.SharedApplication;
            app.InvokeOnMainThread(() =>
            {
                alert = alertFunc();
                var top = this.viewControllerFunc();
                if (alert.PreferredStyle == UIAlertControllerStyle.ActionSheet && UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                {
                    var x = top.View.Bounds.Width / 2;
                    var y = top.View.Bounds.Bottom;
                    var rect = new CGRect(x, y, 0, 0);

                    alert.PopoverPresentationController.SourceView = top.View;
                    alert.PopoverPresentationController.SourceRect = rect;
                    alert.PopoverPresentationController.PermittedArrowDirections = UIPopoverArrowDirection.Unknown;
                }
                top.PresentViewController(alert, true, null);
            });
            return new DisposableAction(() =>
            {
                try
                {
                    app.InvokeOnMainThread(() => alert.DismissViewController(true, null));
                }
                catch { }
            });
        }


        protected virtual IDisposable Present(UIViewController controller)
        {
            var app = UIApplication.SharedApplication;
            var top = this.viewControllerFunc();

            app.InvokeOnMainThread(() => top.PresentViewController(controller, true, null));
            return new DisposableAction(() =>
            {
                try
                {
                    app.InvokeOnMainThread(() => controller.DismissViewController(true, null));
                }
                catch { }
            });
        }


        protected virtual void SetInputType(UITextField txt, InputType inputType)
        {
            switch (inputType)
            {
                case InputType.DecimalNumber:
                    txt.KeyboardType = UIKeyboardType.DecimalPad;
                    break;

                case InputType.Email:
                    txt.KeyboardType = UIKeyboardType.EmailAddress;
                    break;

                case InputType.Name:
                    break;

                case InputType.Number:
                    txt.KeyboardType = UIKeyboardType.NumberPad;
                    break;

                case InputType.NumericPassword:
                    txt.SecureTextEntry = true;
                    txt.KeyboardType = UIKeyboardType.NumberPad;
                    break;

                case InputType.Password:
                    txt.SecureTextEntry = true;
                    break;

                case InputType.Phone:
                    txt.KeyboardType = UIKeyboardType.PhonePad;
                    break;

                case InputType.Url:
                    txt.KeyboardType = UIKeyboardType.Url;
                    break;
            }
        }

        #endregion
    }
}
