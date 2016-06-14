using System;
using System.Linq;
using System.Threading.Tasks;
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
        public override IDisposable Alert(AlertConfig config)
        {
            var alert = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnOk?.Invoke()));
            return this.Present(alert);
        }


        public override IDisposable ActionSheet(ActionSheetConfig config)
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

            return this.Present(sheet);
        }


        public override IDisposable Confirm(ConfirmConfig config)
        {
            var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
            dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x => config.OnConfirm(false)));
            dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnConfirm(true)));
            return this.Present(dlg);
        }


        public override IDisposable DatePrompt(DatePromptConfig config)
        {
            var top = UIApplication.SharedApplication.GetTopViewController();
            var picker = new DatePickerController(config, top)
            {
                ProvidesPresentationContextTransitionStyle = true,
                DefinesPresentationContext = true,
                ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext
            };
            return this.Present(top, picker);
        }


        public override IDisposable TimePrompt(TimePromptConfig config)
        {
            var top = UIApplication.SharedApplication.GetTopViewController();
            var picker = new TimePickerController(config, top)
            {
                ProvidesPresentationContextTransitionStyle = true,
                DefinesPresentationContext = true,
                ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext
            };
            return this.Present(top, picker);
        }


        public override IDisposable Login(LoginConfig config)
        {
            UITextField txtUser = null;
            UITextField txtPass = null;

            var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
            dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x => config.OnResult(new LoginResult(false, txtUser.Text, txtPass.Text))));
            dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnResult(new LoginResult(true, txtUser.Text, txtPass.Text))));

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
            return this.Present(dlg);
        }


        public override IDisposable Prompt(PromptConfig config)
        {
            var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
            UITextField txt = null;

            if (config.IsCancellable)
            {
                dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x =>
                    config.OnResult(new PromptResult(false, txt.Text.Trim())
                )));
            }
            dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x =>
                config.OnResult(new PromptResult(true, txt.Text.Trim())
            )));
            dlg.AddTextField(x =>
            {
                this.SetInputType(x, config.InputType);
                x.Placeholder = config.Placeholder ?? String.Empty;
                if (config.Text != null)
                    x.Text = config.Text;

                txt = x;
            });
            return this.Present(dlg);
        }


        public override void ShowImage(IBitmap image, string message, int timeoutMillis)
        {
            this.ShowWithOverlay(timeoutMillis, () => BTProgressHUD.ShowImage(image.ToNative(), message, timeoutMillis));
        }


        public override void ShowError(string message, int timeoutMillis)
        {
            //this.ShowWithOverlay(timeoutMillis, () => BTProgressHUD.ShowErrorWithStatus(message, timeoutMillis));
            this.ShowWithOverlay(timeoutMillis, () => BTProgressHUD.ShowImage(ProgressHUD.Shared.ErrorImage, message, timeoutMillis));
        }


        public override void ShowSuccess(string message, int timeoutMillis)
        {
            //this.ShowWithOverlay(timeoutMillis, () => BTProgressHUD.ShowSuccessWithStatus(message, timeoutMillis));
            this.ShowWithOverlay(timeoutMillis, () => BTProgressHUD.ShowImage(ProgressHUD.Shared.SuccessImage, message, timeoutMillis));
        }


        IDisposable currentToast;
        public override IDisposable Toast(ToastConfig cfg)
        {
            this.currentToast?.Dispose();

            var snackbar = new TTGSnackbar
            {
                Message = cfg.Message,
                MessageTextColor = cfg.MessageTextColor.ToNative(),
                Duration = cfg.Duration,
                AnimationType = TTGSnackbarAnimationType.FadeInFadeOut
            };

            if (cfg.PrimaryAction != null)
            {
                var color = cfg.PrimaryAction.TextColor ?? ToastConfig.DefaultPrimaryTextColor;

                snackbar.ActionText = cfg.PrimaryAction.Text;
                snackbar.ActionTextColor = color.ToNative();
                snackbar.ActionBlock = x => 
                {
                    snackbar.Dismiss();
                    cfg.PrimaryAction.Action?.Invoke();
                };
            }

            if (cfg.SecondaryAction != null)
            {
                var color = cfg.SecondaryAction.TextColor ?? ToastConfig.DefaultSecondaryTextColor;

                snackbar.SecondActionText = cfg.SecondaryAction.Text;
                snackbar.SecondActionTextColor = color.ToNative();
                snackbar.SecondActionBlock = x => 
                {
                    snackbar.Dismiss();
                    cfg.SecondaryAction.Action?.Invoke();
                };
            }

            var app = UIApplication.SharedApplication;
            app.InvokeOnMainThread(snackbar.Show);

            this.currentToast = new DisposableAction(
                () => app.InvokeOnMainThread(() => snackbar.Dismiss())
            );
            return this.currentToast;
        }


        #region Internals

        UIView currentOverlay;


        protected virtual void ShowWithOverlay(int timemillis, Action action)
        {
            var app = UIApplication.SharedApplication;
            app.InvokeOnMainThread(() =>
            {
                this.ShowOverlay();
                action();
            });
            Task.Delay(timemillis)
                .ContinueWith(x => app.InvokeOnMainThread(this.DismissOverlay));
        }


        protected virtual void ShowOverlay()
        {
            this.currentOverlay = new UIView(UIScreen.MainScreen.Bounds)
            {
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
                Alpha = 0.7F,
                BackgroundColor = UIColor.Black,
                UserInteractionEnabled = false
            };
            UIApplication
                .SharedApplication
                .GetTopWindow()
                .AddSubview(this.currentOverlay);
        }


        protected virtual void DismissOverlay()
        {
            this.currentOverlay?.RemoveFromSuperview();
            this.currentOverlay = null;
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


        protected override IProgressDialog CreateDialogInstance()
        {
            return new ProgressDialog();
        }


        protected virtual IDisposable Present(UIAlertController alert)
        {
            var app = UIApplication.SharedApplication;
            app.InvokeOnMainThread(() =>
            {
                var top = app.GetTopViewController();
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


        protected virtual IDisposable Present(UIViewController presenter, UIViewController controller)
        {
            var app = UIApplication.SharedApplication;
            app.InvokeOnMainThread(() => presenter.PresentViewController(controller, true, null));
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
