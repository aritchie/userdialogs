using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Acr.Support.iOS;
using UIKit;
using BigTed;
using CoreGraphics;
using Foundation;
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


        public override void DatePrompt(DatePromptConfig config)
        {
            var app = UIApplication.SharedApplication;
            var top = app.GetTopViewController();
            var picker = new DatePickerController(config, top)
            {
                ProvidesPresentationContextTransitionStyle = true,
                DefinesPresentationContext = true,
                ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext
            };
            app.InvokeOnMainThread(() => top.PresentViewController(picker, true, null));
        }


        public override void TimePrompt(TimePromptConfig config)
        {
            var app = UIApplication.SharedApplication;
            var top = app.GetTopViewController();
            var picker = new TimePickerController(config, top)
            {
                ProvidesPresentationContextTransitionStyle = true,
                DefinesPresentationContext = true,
                ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext
            };
            app.InvokeOnMainThread(() => top.PresentViewController(picker, true, null));
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

		// PromptTwoInputs added by Lee Bettridge
		public override void Prompt(PromptConfig config)
		{
			var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);

			UITextField txt = null, txt2 = null;

			if (config.IsCancellable)
			{
				if (config.ShowSecondInput)
				{
					dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x =>
						config.OnResult(new PromptResult(false, txt.Text.Trim(), txt2.Text.Trim())
					)));
				}
				else
				{
					dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x =>
						config.OnResult(new PromptResult(false, txt.Text.Trim())
					)));
				}
			}

			if (config.ShowSecondInput)
			{
				dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x =>
					config.OnResult(new PromptResult(true, txt.Text.Trim(), txt2.Text.Trim())
				)));
			}
			else
			{
				dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x =>
					config.OnResult(new PromptResult(true, txt.Text.Trim())
				)));
			}

			dlg.AddTextField(x =>
			{
				this.SetInputType(x, config.InputType);
				x.Placeholder = config.Placeholder ?? String.Empty;

				if (config.Text != null)
					x.Text = config.Text;

				txt = x;
			});

			if (config.ShowSecondInput)
			{
				dlg.AddTextField(x =>
				{
					this.SetInputType(x, config.SecondInputType);
					x.Placeholder = config.SecondPlaceholder ?? String.Empty;

					txt2 = x;
				});

				this.Present(dlg, true);
			}
			else
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
            {
                ProgressHUD.Shared.ShowContinuousProgress(message, ProgressHUD.MaskType.Black, timeoutMillis, ProgressHUD.Shared.ErrorImage);
            });
            Task.Delay(timeoutMillis).ContinueWith(x => ProgressHUD.Shared.Dismiss());
        }


        public override void ShowSuccess(string message, int timeoutMillis)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
                ProgressHUD.Shared.ShowContinuousProgress(message, ProgressHUD.MaskType.Black, timeoutMillis, ProgressHUD.Shared.SuccessImage)
            );
            Task.Delay(timeoutMillis).ContinueWith(x => ProgressHUD.Shared.Dismiss());
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
                alertAction.SetValueForKey(opt.ItemIcon.ToNative(), new NSString("image"));

            controller.AddAction(alertAction);
        }


        protected override IProgressDialog CreateDialogInstance()
        {
            return new ProgressDialog();
        }


        protected virtual void Present(UIAlertController alert, bool secondInput = false)
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