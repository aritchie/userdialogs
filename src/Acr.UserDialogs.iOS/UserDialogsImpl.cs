using System;
using System.Linq;
using BigTed;
#if __UNIFIED__
using CoreGraphics;
using UIKit;
#else
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
#endif


namespace Acr.UserDialogs {

    public class UserDialogsImpl : AbstractUserDialogs {

        public override void Alert(AlertConfig config) {
            UIApplication.SharedApplication.InvokeOnMainThread(() => {
                if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0)) {
                    var alert = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, x => {
                        if (config.OnOk != null)
                            config.OnOk();
                    }));
                    this.Present(alert);
                }
                else {
                    var dlg = new UIAlertView(config.Title ?? String.Empty, config.Message, null, null, config.OkText);
                    if (config.OnOk != null) 
                        dlg.Clicked += (s, e) => config.OnOk();

                    dlg.Show();
                }
            });
        }


        public override void ActionSheet(ActionSheetConfig config) {
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
				this.ShowIOS8ActionSheet(config);
			else
				this.ShowIOS7ActionSheet(config);
        }


        public override void Confirm(ConfirmConfig config) {
            UIApplication.SharedApplication.InvokeOnMainThread(() => {
                if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0)) {
                    var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
                    dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnConfirm(true)));
                    dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Default, x => config.OnConfirm(false)));
                    this.Present(dlg);
                }
                else {
                    var dlg = new UIAlertView(config.Title ?? String.Empty, config.Message, null, config.CancelText, config.OkText);
                    dlg.Clicked += (s, e) => {
                        var ok = ((int)dlg.CancelButtonIndex != (int)e.ButtonIndex);
                        config.OnConfirm(ok);
                    };
                    dlg.Show();
                }
            });
        }


        public override void Login(LoginConfig config) {
            UITextField txtUser = null;
            UITextField txtPass = null;

            UIApplication.SharedApplication.InvokeOnMainThread(() => {

                if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0)) {
                    var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
                    dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, true))));
                    dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Default, x => config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, false))));

                    dlg.AddTextField(x => {
                        txtUser = x;
                        x.Placeholder = config.LoginPlaceholder;
                        x.Text = config.LoginValue ?? String.Empty;
                    });
                    dlg.AddTextField(x => {
                        txtPass = x;
                        x.Placeholder = config.PasswordPlaceholder;
                        x.SecureTextEntry = true;
                    });
                    this.Present(dlg);
                }
                else {
                    var dlg = new UIAlertView { AlertViewStyle = UIAlertViewStyle.LoginAndPasswordInput };
                    txtUser = dlg.GetTextField(0);
                    txtPass = dlg.GetTextField(1);

                    txtUser.Placeholder = config.LoginPlaceholder;
                    txtUser.Text = config.LoginValue ?? String.Empty;
                    txtPass.Placeholder = config.PasswordPlaceholder;

                    dlg.Clicked += (s, e) => {
                        var ok = ((int)dlg.CancelButtonIndex != (int)e.ButtonIndex);
                        config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, ok));
                    };
                    dlg.Show();
                }
            });
        }


        public override void Prompt(PromptConfig config) {
            UIApplication.SharedApplication.InvokeOnMainThread(() => {
				if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0)) 
					this.ShowIOS8Prompt(config);
                else 
					this.ShowIOS7Prompt(config);
            });
        }


        public override void Toast(string message, int timeoutSeconds = 3, Action onClick = null) {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>  {
                var ms = timeoutSeconds * 1000;
                BTProgressHUD.ShowToast(message, false, ms);
            });
        }


		protected virtual void AddActionSheetOption(ActionSheetOption opt, UIAlertController controller, UIAlertActionStyle style) {
			controller.AddAction(UIAlertAction.Create(opt.Text, style, x => opt.TryExecute()));
		}


        protected override IProgressDialog CreateDialogInstance() {
            return new ProgressDialog();
        }


		protected virtual void ShowIOS7ActionSheet(ActionSheetConfig config) {
			var view = this.GetTopView();
			var action = new UIActionSheet(config.Title);
			config.Options.ToList().ForEach(x => action.AddButton(x.Text));
			var index = config.Options.Count - 1;

			if (config.Destructive != null) {
				index++;
				action.AddButton(config.Destructive.Text);
				action.DestructiveButtonIndex = index;
			}

			if (config.Cancel != null) {
				index++;
				action.AddButton(config.Cancel.Text);
				action.CancelButtonIndex = index;
			}

			action.Dismissed += (sender, btn) => {
				if (btn.ButtonIndex == action.DestructiveButtonIndex) 
					config.Destructive.TryExecute();

				else if (btn.ButtonIndex == action.CancelButtonIndex)
					config.Cancel.TryExecute();

				else if (btn.ButtonIndex > -1)
					config.Options[(int)btn.ButtonIndex].TryExecute();
			};
			action.ShowInView(view);
		}


		protected virtual void ShowIOS8ActionSheet(ActionSheetConfig config) {
			var sheet = UIAlertController.Create(config.Title ?? String.Empty, String.Empty, UIAlertControllerStyle.ActionSheet);
			config
				.Options
				.ToList()
				.ForEach(x => this.AddActionSheetOption(x, sheet, UIAlertActionStyle.Default));

			if (config.Destructive != null)
				this.AddActionSheetOption(config.Destructive, sheet, UIAlertActionStyle.Destructive);

			if (config.Cancel != null)
				this.AddActionSheetOption(config.Cancel, sheet, UIAlertActionStyle.Cancel);

			this.Present(sheet);
		}


		protected virtual void ShowIOS7Prompt(PromptConfig config) {
			var result = new PromptResult();
			var isPassword = config.InputType == InputType.Password;
			var cancelText = config.IsCancellable ? config.CancelText : null;

			var dlg = new UIAlertView(config.Title ?? String.Empty, config.Message, null, cancelText, config.OkText) {
				AlertViewStyle = isPassword
					? UIAlertViewStyle.SecureTextInput
					: UIAlertViewStyle.PlainTextInput
			};
			var txt = dlg.GetTextField(0);
			this.SetInputType(txt, config.InputType);
			txt.Placeholder = config.Placeholder;
			if (config.Text != null)
				txt.Text = config.Text;

			dlg.Clicked += (s, e) => {
				result.Ok = ((int)dlg.CancelButtonIndex != (int)e.ButtonIndex);
				result.Text = txt.Text.Trim();
				config.OnResult(result);
			};
			dlg.Show();
		}


		protected virtual void ShowIOS8Prompt(PromptConfig config) {
			var result = new PromptResult();
			var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
			UITextField txt = null;

			dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => {
				result.Ok = true;
				result.Text = txt.Text.Trim();
				config.OnResult(result);
			}));
			if (config.IsCancellable) {
				dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Default, x => {
					result.Ok = false;
					result.Text = txt.Text.Trim();
					config.OnResult(result);
				}));
			}
			dlg.AddTextField(x => {
				this.SetInputType(x, config.InputType);
				x.Placeholder = config.Placeholder ?? String.Empty;
				if (config.Text != null)
					x.Text = config.Text;

				txt = x;
			});
			this.Present(dlg);
		}


        protected virtual void Present(UIAlertController controller) {
            UIApplication.SharedApplication.InvokeOnMainThread(() => {
                var top = this.GetTopViewController();
                var po = controller.PopoverPresentationController;
                if (po != null) {
                    po.SourceView = top.View;
                    var h = (top.View.Frame.Height / 2) - 400;
                    var v = (top.View.Frame.Width / 2) - 300;
#if __UNIFIED__
                    po.SourceRect = new CGRect(v, h, 0, 0);
#else
                    po.SourceRect = new System.Drawing.RectangleF(v, h, 0, 0);
#endif
                    po.PermittedArrowDirections = UIPopoverArrowDirection.Any;
                }
                top.PresentViewController(controller, true, null);
            });
        }


        protected virtual void SetInputType(UITextField txt, InputType inputType) {
            switch (inputType) {
                case InputType.Email  :
                    txt.KeyboardType = UIKeyboardType.EmailAddress;
                    break;

                case InputType.Number: 
                    txt.KeyboardType = UIKeyboardType.NumberPad;
                    break;

                case InputType.Password:
                    txt.SecureTextEntry = true;
                    break;

                default :
                    txt.KeyboardType = UIKeyboardType.Default;
                    break;
            }
        }


        protected virtual UIWindow GetTopWindow() {
            return UIApplication.SharedApplication
                .Windows
                .Reverse()
                .FirstOrDefault(x => 
                    x.WindowLevel == UIWindowLevel.Normal && 
                    !x.Hidden
                );
        }


        protected virtual UIView GetTopView() {
            return this.GetTopWindow().Subviews.Last();
        }


        protected virtual UIViewController GetTopViewController() {
            var root = this.GetTopWindow().RootViewController;
            var tabs = root as UITabBarController;
            if (tabs != null)
                return tabs.SelectedViewController;

            var nav = root as UINavigationController;
            if (nav != null)
                return nav.VisibleViewController;

            if (root.PresentedViewController != null)
                return root.PresentedViewController;

            return root;
        }
    }
}

        //public override void DateTimePrompt(DateTimePromptConfig config) {
        //    var sheet = new ActionSheetDatePicker {
        //        Title = config.Title,
        //        DoneText = config.OkText
        //    };

        //    switch (config.SelectionType) {
        //        case DateTimeSelectionType.Date:
        //            sheet.DatePicker.Mode = UIDatePickerMode.Date;
        //            break;

        //        case DateTimeSelectionType.Time:
        //            sheet.DatePicker.Mode = UIDatePickerMode.Time;
        //            break;

        //        case DateTimeSelectionType.DateTime:
        //            sheet.DatePicker.Mode = UIDatePickerMode.DateAndTime;
        //            break;
        //    }
        //    if (config.MinValue != null)
        //        sheet.DatePicker.MinimumDate = config.MinValue.Value;

        //    if (config.MaxValue != null)
        //        sheet.DatePicker.MaximumDate = config.MaxValue.Value;

        //    sheet.DateTimeSelected += (sender, args) => {
        //        // TODO: stop adjusting date/time
        //        config.OnResult(new DateTimePromptResult(sheet.DatePicker.Date));
        //    };

        //    var top = Utils.GetTopView();
        //    sheet.Show(top);
        //    //sheet.DatePicker.MinuteInterval
        //}


        //public override void DurationPrompt(DurationPromptConfig config) {
        //    var sheet = new ActionSheetDatePicker {
        //        Title = config.Title,
        //        DoneText = config.OkText
        //    };
        //    sheet.DatePicker.Mode = UIDatePickerMode.CountDownTimer;

        //    sheet.DateTimeSelected += (sender, args) => config.OnResult(new DurationPromptResult(args.TimeOfDay));

        //    var top = Utils.GetTopView();
        //    sheet.Show(top);
        //}