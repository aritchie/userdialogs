using System;
using System.Linq;
using System.Text;
using UIKit;
using CoreGraphics;
using Foundation;
using Acr.Support.iOS;
using BigTed;
using Splat;
using TTG;


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


        public override IDisposable Alert(AlertConfig config) => this.Present(() =>
        {
            var alert = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnAction?.Invoke()));
            return alert;
        });


        public override IDisposable ActionSheet(ActionSheetConfig config) => this.Present(() => this.CreateNativeActionSheet(config));


        public override IDisposable Confirm(ConfirmConfig config) => this.Present(() =>
        {
            var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
            dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x => config.OnAction?.Invoke(false)));
            dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnAction?.Invoke(true)));
            return dlg;
        });


        public override IDisposable DatePrompt(DatePromptConfig config)
        {
            var picker = new AI.AIDatePickerController
            {
                Mode = UIDatePickerMode.Date,
                SelectedDateTime = config.SelectedDate ?? DateTime.Now,
                OkText = config.OkText,
                CancelText = config.CancelText,
                Ok = x => config.OnAction?.Invoke(new DatePromptResult(true, x.SelectedDateTime)),
                Cancel = x => config.OnAction?.Invoke(new DatePromptResult(false, x.SelectedDateTime)),
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
                Ok = x => config.OnAction?.Invoke(new TimePromptResult(true, x.SelectedDateTime.TimeOfDay)),
                Cancel = x => config.OnAction?.Invoke(new TimePromptResult(false, x.SelectedDateTime.TimeOfDay)),
                Use24HourClock = config.Use24HourClock
            };
            return this.Present(picker);
        }


        public override IDisposable Login(LoginConfig config) => this.Present(() =>
        {
            UITextField txtUser = null;
            UITextField txtPass = null;

            var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
            dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x => config.OnAction?.Invoke(new LoginResult(false, txtUser.Text, txtPass.Text))));
            dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnAction?.Invoke(new LoginResult(true, txtUser.Text, txtPass.Text))));
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


        public override IDisposable Prompt(PromptConfig config) => this.Present(() =>
        {
            var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
            UITextField txt = null;

            if (config.IsCancellable)
            {
                dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x =>
                    config.OnAction?.Invoke(new PromptResult(false, txt.Text)
                )));
            }

            var btnOk = UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x =>
                config.OnAction?.Invoke(new PromptResult(true, txt.Text)
            ));
            dlg.AddAction(btnOk);

            dlg.AddTextField(x =>
            {
                txt = x;
                this.SetInputType(txt, config.InputType);
                txt.Placeholder = config.Placeholder ?? String.Empty;
                txt.Text = config.Text ?? String.Empty;

                if (config.MaxLength != null)
                {
                    txt.ShouldChangeCharacters = (field, replacePosition, replacement) =>
                    {
                        var updatedText = new StringBuilder(field.Text);
                        updatedText.Remove((int)replacePosition.Location, (int)replacePosition.Length);
                        updatedText.Insert((int)replacePosition.Location, replacement);
                        return updatedText.ToString().Length <= config.MaxLength.Value;
                    };
                }

                if (config.OnTextChanged != null)
                {
                    txt.AddTarget((sender, e) => ValidatePrompt(txt, btnOk, config), UIControlEvent.EditingChanged);
                    ValidatePrompt(txt, btnOk, config);
                }
            });
            return dlg;
        });


        static void ValidatePrompt(UITextField txt, UIAlertAction btn, PromptConfig config)
        {
            var args = new PromptTextChangedArgs { Value = txt.Text };
            config.OnTextChanged(args);
            btn.Enabled = args.IsValid;
            if (!txt.Text.Equals(args.Value))
                txt.Text = args.Value;
        }


        public override void ShowImage(IBitmap image, string message, int timeoutMillis)
            => BTProgressHUD.ShowImage(image.ToNative(), message, timeoutMillis);


        public override void ShowError(string message, int timeoutMillis)
            => BTProgressHUD.ShowErrorWithStatus(message, timeoutMillis);


        public override void ShowSuccess(string message, int timeoutMillis)
            => BTProgressHUD.ShowSuccessWithStatus(message, timeoutMillis);


        IDisposable currentToast;
        public override IDisposable Toast(ToastConfig cfg)
        {
            this.currentToast?.Dispose();

            var app = UIApplication.SharedApplication;
            app.InvokeOnMainThread(() =>
            {
                //var snackbar = new TTGSnackbar(cfg.Message)
                var snackbar = new TTGSnackbar
                {
                    Message = cfg.Message,
                    Duration = cfg.Duration,
                    AnimationType = TTGSnackbarAnimationType.FadeInFadeOut,
                    ShowOnTop = cfg.Position == ToastPosition.Top
                };
                if (cfg.Icon != null)
                    snackbar.Icon = cfg.Icon.ToNative();

                if (cfg.BackgroundColor != null)
                    snackbar.BackgroundColor = cfg.BackgroundColor.Value.ToNative();

                if (cfg.MessageTextColor != null)
                    snackbar.MessageLabel.TextColor = cfg.MessageTextColor.Value.ToNative();
                    //snackbar.MessageTextColor = cfg.MessageTextColor.Value.ToNative();

                //if (cfg.Position != null)
                //    snackbar.LocationType = cfg.Position == ToastPosition.Top
                //        ? TTGSnackbarLocation.Top
                //        : TTGSnackbarLocation.Bottom;

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
            var sheet = UIAlertController.Create(config.Title, config.Message, UIAlertControllerStyle.ActionSheet);

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


        protected override IProgressDialog CreateDialogInstance(ProgressDialogConfig config) => new ProgressDialog(config);


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

        public override IDisposable NumberPrompt(NumberPromptConfig config)
        {
          /*  //picker
            UIPickerView pickerView = new UIPickerView(
                            new CGRect(
                                UIScreen.MainScreen.Bounds.X - UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height - 230,
                                UIScreen.MainScreen.Bounds.Width,
                                180));

            NumberModelPicker model = new NumberModelPicker(config);
            pickerView.Model = model;

            model.ValueChanged += (sender, e) => {
                config.SelectedNumber = model.SelectedItem;
            };

            //toolbar
            UIToolbar toolbar = new UIToolbar();
            toolbar.BarStyle = UIBarStyle.Default;
            toolbar.Translucent = true;
            toolbar.SizeToFit();

            UIBarButtonItem doneButton = new UIBarButtonItem(config.OkText,
                                                             UIBarButtonItemStyle.Done,
                                                             (s, e) => config.OnAction?.Invoke(new NumberPromptResult(true, config.SelectedNumber.Value)));
            
            toolbar.SetItems(new UIBarButtonItem[] { doneButton }, true);

            pickerView.AddSubview(toolbar);

            //controller
            var controller = new UIViewController();

            controller.Add(pickerView);
            controller.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            controller.View.BackgroundColor = UIColor.Clear;*/
            var picker = new AI.AINumberPickerController
            {
                OkText = config.OkText,
                CancelText = config.CancelText,
                Ok = x => config.OnAction?.Invoke(new NumberPromptResult(true, x.SelectedNumber)),
                Cancel = x => config.OnAction?.Invoke(new NumberPromptResult(false, x.SelectedNumber)),
            };

            if (config.MinNumber!=null){
                picker.MinNumber = config.MinNumber.Value;
            }

            if (config.MaxNumber != null)
            {
                picker.MaxNumber = config.MaxNumber.Value;
            }

            if(config.SelectedNumber != null)
            {
                picker.SelectedNumber = config.SelectedNumber.Value;
            }


            return this.Present(picker);

        }

        #endregion
    }
}
