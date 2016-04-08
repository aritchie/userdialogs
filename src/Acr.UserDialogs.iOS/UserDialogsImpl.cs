using System;
using System.Linq;
using System.Timers;
using Acr.Support.iOS;
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
        protected static bool IsIOS8 => UIDevice.CurrentDevice.CheckSystemVersion(8, 0);


        public override void Alert(AlertConfig config)
        {
            if (IsIOS8)
            {
                var alert = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnOk?.Invoke()));
                this.Present(alert);
            }
            else
            {
                var dlg = new UIAlertView(config.Title ?? String.Empty, config.Message, null, null, config.OkText);
                dlg.Clicked += (s, e) => config.OnOk?.Invoke();
                this.Present(dlg);
            }
        }


        public override void ActionSheet(ActionSheetConfig config)
        {
            if (IsIOS8)
                this.ShowIOS8ActionSheet(config);
            else
                this.ShowIOS7ActionSheet(config);
        }


        public override void Confirm(ConfirmConfig config)
        {
            if (IsIOS8)
            {
                var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
                dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x => config.OnConfirm(false)));
                dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnConfirm(true)));
                this.Present(dlg);
            }
            else
            {
                var dlg = new UIAlertView(config.Title ?? String.Empty, config.Message, null, config.CancelText, config.OkText);
                dlg.Clicked += (s, e) =>
                {
                    var ok = ((int)dlg.CancelButtonIndex != (int)e.ButtonIndex);
                    config.OnConfirm(ok);
                };
                this.Present(dlg);
            }
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
            if (IsIOS8)
                this.ShowIOS8Login(config);
            else
                this.ShowIOS7Login(config);
        }


        public override void Prompt(PromptConfig config)
        {
            if (IsIOS8)
                this.ShowIOS8Prompt(config);
            else
                this.ShowIOS7Prompt(config);
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

        #region Implementations

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


        protected virtual void ShowIOS7ActionSheet(ActionSheetConfig config)
        {
            var view = UIApplication.SharedApplication.GetTopView();
            var action = new UIActionSheet(config.Title);
            config.Options.ToList().ForEach(x => action.AddButton(x.Text));
            var index = config.Options.Count - 1;

            if (config.Destructive != null)
            {
                index++;
                action.AddButton(config.Destructive.Text);
                action.DestructiveButtonIndex = index;
            }

            if (config.Cancel != null)
            {
                index++;
                action.AddButton(config.Cancel.Text);
                action.CancelButtonIndex = index;
            }

            action.Dismissed += (sender, btn) =>
            {
                if (btn.ButtonIndex == action.DestructiveButtonIndex)
                    config.Destructive.Action?.Invoke();

                else if (btn.ButtonIndex == action.CancelButtonIndex)
                    config.Cancel.Action?.Invoke();

                else if (btn.ButtonIndex > -1)
                    config.Options[(int)btn.ButtonIndex].Action?.Invoke();
            };
            UIApplication.SharedApplication.InvokeOnMainThread(() => action.ShowInView(view));
        }


        protected virtual void ShowIOS8ActionSheet(ActionSheetConfig config)
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


        protected virtual void ShowIOS7Login(LoginConfig config)
        {
            var dlg = new UIAlertView
            {
                AlertViewStyle = UIAlertViewStyle.LoginAndPasswordInput,
                Title = config.Title,
                Message = config.Message
            };
            var txtUser = dlg.GetTextField(0);
            var txtPass = dlg.GetTextField(1);

            txtUser.Placeholder = config.LoginPlaceholder;
            txtUser.Text = config.LoginValue ?? String.Empty;
            txtPass.Placeholder = config.PasswordPlaceholder;

            dlg.AddButton(config.OkText);
            dlg.AddButton(config.CancelText);
            dlg.CancelButtonIndex = 1;

            dlg.Clicked += (s, e) =>
            {
                var ok = ((int)dlg.CancelButtonIndex != (int)e.ButtonIndex);
                config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, ok));
            };
            this.Present(dlg);
        }


        protected virtual void ShowIOS8Login(LoginConfig config)
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


        protected virtual void ShowIOS7Prompt(PromptConfig config)
        {

            var isPassword = (config.InputType == InputType.Password || config.InputType == InputType.NumericPassword);
            var cancelText = config.IsCancellable ? config.CancelText : null;

            var dlg = new UIAlertView(config.Title ?? String.Empty, config.Message, null, cancelText, config.OkText)
            {
                AlertViewStyle = isPassword
                    ? UIAlertViewStyle.SecureTextInput
                    : UIAlertViewStyle.PlainTextInput
            };
            var txt = dlg.GetTextField(0);
            this.SetInputType(txt, config.InputType);
            txt.Placeholder = config.Placeholder;
            if (config.Text != null)
                txt.Text = config.Text;

            dlg.Clicked += (s, e) =>
            {
                var ok = ((int)dlg.CancelButtonIndex != (int)e.ButtonIndex);
                var text = txt.Text.Trim();
                config.OnResult(new PromptResult(ok, text));
            };
            this.Present(dlg);
        }


        protected virtual void ShowIOS8Prompt(PromptConfig config)
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


        protected virtual void Present(UIAlertView alert)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(alert.Show);
        }


        protected virtual void Present(UIAlertController alert)
        {
            UIApplication.SharedApplication.Present(alert);
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