using System;
using System.Linq;
using System.Timers;
using UIKit;
using BigTed;
using MessageBar;
using Splat;


namespace Acr.UserDialogs
{

    public class UserDialogsImpl : AbstractUserDialogs
    {
        readonly Timer toastTimer;


        public UserDialogsImpl()
        {
            this.toastTimer = new Timer();
            this.toastTimer.Elapsed += (sender, args) =>
            {
                this.toastTimer.Stop();
                UIApplication.SharedApplication.InvokeOnMainThread(MessageBarManager.SharedInstance.HideAll);
            };
        }


        public static bool ShowToastOnBottom { get; set; }


        public override void Alert(AlertConfig config)
        {
            var alert = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnOk?.Invoke()));
            this.Present(alert);
        }


        public override void ActionSheet(ActionSheetConfig config)
        {
            var sheet = UIAlertController.Create(config.Title, null, UIAlertControllerStyle.ActionSheet);
            config
                .Options
                .ToList()
                .ForEach(x => this.AddActionSheetOption(x, sheet, UIAlertActionStyle.Default, config.ItemIcon));

            if (config.Destructive != null)
                this.AddActionSheetOption(config.Destructive, sheet, UIAlertActionStyle.Destructive);

            if (config.Cancel != null)
                this.AddActionSheetOption(config.Cancel, sheet, UIAlertActionStyle.Cancel);

            this.Present(sheet);
        }


        public override void Confirm(ConfirmConfig config)
        {
            var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
            dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x => config.OnConfirm(false)));
            dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnConfirm(true)));
            this.Present(dlg);
        }


        public override void DateTimePrompt(DateTimePromptConfig config)
        {
            var viewController = new DatePickerController(config);
            //var pop = new UIPopoverController(vc)
            //{
            //    PopoverContentSize = new CGSize(200, 100)
            //};
            var app = UIApplication.SharedApplication;
            app.InvokeOnMainThread(() =>
                //pop.PresentFromRect(new CGRect(0, 0, 200, 100), null, UIPopoverArrowDirection.Any, true))
                app.KeyWindow.RootViewController.PresentViewController(viewController, true, null)
            );
        }



        public override void Login(LoginConfig config)
        {
            UITextField txtUser = null;
            UITextField txtPass = null;

            var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
            dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x => config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, false))));
            dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, true))));

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
            this.Present(dlg);
        }


        public override void Prompt(PromptConfig config)
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
                config.OnResult(new PromptResult(false, txt.Text.Trim())
            )));
            dlg.AddTextField(x =>
            {
                this.SetInputType(x, config.InputType);
                x.Placeholder = config.Placeholder ?? String.Empty;
                if (config.Text != null)
                    x.Text = config.Text;

                txt = x;
            });
            this.Present(dlg);
        }


        public override void ShowImage(IBitmap image, string message, int timeoutMillis)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
                BTProgressHUD.ShowImage(image.ToNative(), message, timeoutMillis)
            );
        }


        public override void ShowError(string message, int timeoutMillis)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
                BTProgressHUD.ShowErrorWithStatus(message, timeoutMillis)
            );
        }


        public override void ShowSuccess(string message, int timeoutMillis)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
                BTProgressHUD.ShowSuccessWithStatus(message, timeoutMillis)
            );
        }


        public override void Toast(ToastConfig cfg)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                MessageBarManager.SharedInstance.ShowAtTheBottom = ShowToastOnBottom;
                MessageBarManager.SharedInstance.HideAll();
                MessageBarManager.SharedInstance.StyleSheet = new AcrMessageBarStyleSheet(cfg);
                MessageBarManager.SharedInstance.ShowMessage(cfg.Title, cfg.Description ?? String.Empty, MessageType.Success, null, () => cfg.Action?.Invoke());

                this.toastTimer.Stop();
                this.toastTimer.Interval = cfg.Duration.TotalMilliseconds;
                this.toastTimer.Start();
            });
        }

        #region Internals

        protected virtual void AddActionSheetOption(ActionSheetOption opt, UIAlertController controller, UIAlertActionStyle style, IBitmap image = null)
        {
            var alertAction = UIAlertAction.Create(opt.Text, style, x => opt.Action?.Invoke());

            if (opt.ItemIcon == null && image != null)
                opt.ItemIcon = image;

            if (opt.ItemIcon != null)
                alertAction.SetValueForKey(opt.ItemIcon.ToNative(), new Foundation.NSString("image"));

            controller.AddAction(alertAction);
        }


        protected override IProgressDialog CreateDialogInstance()
        {
            return new ProgressDialog();
        }



        protected virtual void Present(UIAlertView alert)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(alert.Show);
        }


        protected virtual void Present(UIAlertController alert)
        {
            var app = UIApplication.SharedApplication;
            app.InvokeOnMainThread(() =>
                app.KeyWindow.RootViewController.PresentViewController(alert, true, null)
            );
            //    PresentInternal(app, controller, animated, action);
            //else
            //    app.InvokeOnMainThread(() => PresentInternal(app, controller, animated, action));
        }



        //static void PresentInternal(UIApplication app, UIViewController controller, bool animated, Action action) {
        //    var top = app.GetTopViewController();
        //    if (controller.PopoverPresentationController != null) {
        //        var x = top.View.Bounds.Width / 2;
        //        var y = top.View.Bounds.Bottom;
        //        var rect = new CGRect(x, y, 0, 0);

        //        controller.PopoverPresentationController.SourceView = top.View;
        //        controller.PopoverPresentationController.SourceRect = rect;
        //        controller.PopoverPresentationController.PermittedArrowDirections = UIPopoverArrowDirection.Unknown;
        //    }
        //    top.PresentViewController(controller, animated, action);
        //}


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