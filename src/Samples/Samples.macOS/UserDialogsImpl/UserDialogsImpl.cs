using System;
using AppKit;

namespace Acr.UserDialogs
{
    public class UserDialogsImpl : AbstractUserDialogs
    {
        readonly Func<NSWindow> windowFunc;

        public UserDialogsImpl() : this(() => NSApplication.SharedApplication.KeyWindow)//.GetTopViewController())
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

        public override IDisposable ActionSheet(ActionSheetConfig config) => null; //this.Present(() => this.CreateNativeActionSheet(config));


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
            throw new NotImplementedException();
            //var picker = new AI.AIDatePickerController
            //{
            //    Mode = UIDatePickerMode.Date,
            //    SelectedDateTime = config.SelectedDate ?? DateTime.Now,
            //    OkText = config.OkText,
            //    CancelText = config.CancelText,
            //    Ok = x => config.OnAction?.Invoke(new DatePromptResult(true, x.SelectedDateTime)),
            //    Cancel = x => config.OnAction?.Invoke(new DatePromptResult(false, x.SelectedDateTime)),
            //};
            //if (config.MaximumDate != null)
            //    picker.MaximumDateTime = config.MaximumDate;

            //if (config.MinimumDate != null)
            //    picker.MinimumDateTime = config.MinimumDate;

            //return this.Present(picker);
        }


        public override IDisposable TimePrompt(TimePromptConfig config)
        {
            throw new NotImplementedException();
            //        var picker = new AI.AIDatePickerController
            //        {
            //            Mode = UIDatePickerMode.Time,
            //SelectedDateTime = config.SelectedTime != null ? DateTime.Today.Add ((TimeSpan)config.SelectedTime) : DateTime.Now,
            //            MinuteInterval = config.MinuteInterval,
            //            OkText = config.OkText,
            //            CancelText = config.CancelText,
            //            Ok = x => config.OnAction?.Invoke(new TimePromptResult(true, x.SelectedDateTime.TimeOfDay)),
            //            Cancel = x => config.OnAction?.Invoke(new TimePromptResult(false, x.SelectedDateTime.TimeOfDay)),
            //            Use24HourClock = config.Use24HourClock
            //        };
            //        return this.Present(picker);
        }


        public override IDisposable Login(LoginConfig config) => this.Present(() =>
        {
            throw new NotImplementedException();

            //UITextField txtUser = null;
            //UITextField txtPass = null;

            //var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
            //dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x => config.OnAction?.Invoke(new LoginResult(false, txtUser.Text, txtPass.Text))));
            //dlg.AddAction(UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x => config.OnAction?.Invoke(new LoginResult(true, txtUser.Text, txtPass.Text))));
            //dlg.AddTextField(x =>
            //{
            //    txtUser = x;
            //    x.Placeholder = config.LoginPlaceholder;
            //    x.Text = config.LoginValue ?? String.Empty;
            //});
            //dlg.AddTextField(x =>
            //{
            //    txtPass = x;
            //    x.Placeholder = config.PasswordPlaceholder;
            //    x.SecureTextEntry = true;
            //});
            //return dlg;
        });


        public override IDisposable Prompt(PromptConfig config) => this.Present(() =>
        {
            throw new NotImplementedException();

            //var dlg = UIAlertController.Create(config.Title ?? String.Empty, config.Message, UIAlertControllerStyle.Alert);
            //UITextField txt = null;

            //if (config.IsCancellable)
            //{
            //    dlg.AddAction(UIAlertAction.Create(config.CancelText, UIAlertActionStyle.Cancel, x =>
            //        config.OnAction?.Invoke(new PromptResult(false, txt.Text)
            //    )));
            //}

            //var btnOk = UIAlertAction.Create(config.OkText, UIAlertActionStyle.Default, x =>
            //    config.OnAction?.Invoke(new PromptResult(true, txt.Text)
            //));
            //dlg.AddAction(btnOk);

            //dlg.AddTextField(x =>
            //{
            //    txt = x;
            //    this.SetInputType(txt, config.InputType);
            //    txt.Placeholder = config.Placeholder ?? String.Empty;
            //    txt.Text = config.Text ?? String.Empty;

            //    if (config.MaxLength != null)
            //    {
            //        txt.ShouldChangeCharacters = (field, replacePosition, replacement) =>
            //        {
            //            var updatedText = new StringBuilder(field.Text);
            //            updatedText.Remove((int)replacePosition.Location, (int)replacePosition.Length);
            //            updatedText.Insert((int)replacePosition.Location, replacement);
            //            return updatedText.ToString().Length <= config.MaxLength.Value;
            //        };
            //    }

            //    if (config.OnTextChanged != null)
            //    {
            //        txt.AddTarget((sender, e) => ValidatePrompt(txt, btnOk, config), UIControlEvent.EditingChanged);
            //        ValidatePrompt(txt, btnOk, config);
            //    }
            //});
            //return dlg;
        });


        //static void ValidatePrompt(UITextField txt, UIAlertAction btn, PromptConfig config)
        //{
        //    var args = new PromptTextChangedArgs { Value = txt.Text };
        //    config.OnTextChanged(args);
        //    btn.Enabled = args.IsValid;
        //    if (!txt.Text.Equals(args.Value))
        //        txt.Text = args.Value;
        //}


        IDisposable currentToast;
        public override IDisposable Toast(ToastConfig cfg)
        {
            throw new NotImplementedException();
            //this.currentToast?.Dispose();

            //var app = UIApplication.SharedApplication;
            //app.SafeInvokeOnMainThread(() =>
            //{
            //    //var snackbar = new TTGSnackbar(cfg.Message)
            //    var snackbar = new TTGSnackbar
            //    {
            //        Message = cfg.Message,
            //        Duration = cfg.Duration,
            //        AnimationType = TTGSnackbarAnimationType.FadeInFadeOut,
            //        ShowOnTop = cfg.Position == ToastPosition.Top
            //    };
            //    if (cfg.Icon != null)
            //        snackbar.Icon = UIImage.FromBundle(cfg.Icon);

            //    if (cfg.BackgroundColor != null)
            //        snackbar.BackgroundColor = cfg.BackgroundColor.Value.ToNative();

            //    if (cfg.MessageTextColor != null)
            //        snackbar.MessageLabel.TextColor = cfg.MessageTextColor.Value.ToNative();
            //        //snackbar.MessageTextColor = cfg.MessageTextColor.Value.ToNative();

            //    //if (cfg.Position != null)
            //    //    snackbar.LocationType = cfg.Position == ToastPosition.Top
            //    //        ? TTGSnackbarLocation.Top
            //    //        : TTGSnackbarLocation.Bottom;

            //    if (cfg.Action != null)
            //    {
            //        var color = cfg.Action.TextColor ?? ToastConfig.DefaultActionTextColor;
            //        if (color != null)
            //            snackbar.ActionButton.SetTitleColor(color.Value.ToNative(), UIControlState.Normal);

            //        snackbar.ActionText = cfg.Action.Text;
            //        snackbar.ActionBlock = x =>
            //        {
            //            snackbar.Dismiss();
            //            cfg.Action.Action?.Invoke();
            //        };
            //    }
            //    snackbar.Show();

            //    this.currentToast = new DisposableAction(
            //        () => app.SafeInvokeOnMainThread(() => snackbar.Dismiss())
            //    );
            //});
            //return this.currentToast;
        }


        #region Internals

        //protected virtual UIAlertController CreateNativeActionSheet(ActionSheetConfig config)
        //{
        //    var sheet = UIAlertController.Create(config.Title, config.Message, UIAlertControllerStyle.ActionSheet);

        //    if (config.Destructive != null)
        //        this.AddActionSheetOption(config.Destructive, sheet, UIAlertActionStyle.Destructive, config.ItemIcon);

        //    config
        //        .Options
        //        .ToList()
        //        .ForEach(x => this.AddActionSheetOption(x, sheet, UIAlertActionStyle.Default, config.ItemIcon));

        //    if (config.Cancel != null)
        //        this.AddActionSheetOption(config.Cancel, sheet, UIAlertActionStyle.Cancel, config.ItemIcon);

        //    return sheet;
        //}

        //protected virtual void AddActionSheetOption(ActionSheetOption opt, UIAlertController controller, UIAlertActionStyle style, string imageName)
        //{
        //    var alertAction = UIAlertAction.Create(opt.Text, style, x => opt.Action?.Invoke());

        //    if (opt.ItemIcon == null && imageName != null)
        //        opt.ItemIcon = imageName;

        //    if (opt.ItemIcon != null)
        //    {
        //        var icon = UIImage.FromBundle(opt.ItemIcon);
        //        alertAction.SetValueForKey(icon, new NSString("image"));
        //    }
        //    controller.AddAction(alertAction);
        //}


        protected override IProgressDialog CreateDialogInstance(ProgressDialogConfig config) => null; // new ProgressDialog(config);


        protected virtual IDisposable Present(Func<NSAlert> alertFunc)
        {
            NSAlert alert = null;
            var app = NSApplication.SharedApplication;
            app.InvokeOnMainThread(() =>
            {
                alert = alertFunc();
            });
            return new DisposableAction(() => app.InvokeOnMainThread(() => alert?.Dispose()));
        }


        //protected virtual IDisposable Present(UIViewController controller)
        //{
        //    var app = UIApplication.SharedApplication;
        //    var top = this.viewControllerFunc();

        //    app.SafeInvokeOnMainThread(() => top.PresentViewController(controller, true, null));
        //    return new DisposableAction(() => app.SafeInvokeOnMainThread(() => controller.DismissViewController(true, null)));
        //}


        //protected virtual void SetInputType(UITextField txt, InputType inputType)
        //{
        //    switch (inputType)
        //    {
        //        case InputType.DecimalNumber:
        //            txt.KeyboardType = UIKeyboardType.DecimalPad;
        //            break;

        //        case InputType.Email:
        //            txt.KeyboardType = UIKeyboardType.EmailAddress;
        //            break;

        //        case InputType.Name:
        //            break;

        //        case InputType.Number:
        //            txt.KeyboardType = UIKeyboardType.NumberPad;
        //            break;

        //        case InputType.NumericPassword:
        //            txt.SecureTextEntry = true;
        //            txt.KeyboardType = UIKeyboardType.NumberPad;
        //            break;

        //        case InputType.Password:
        //            txt.SecureTextEntry = true;
        //            break;

        //        case InputType.Phone:
        //            txt.KeyboardType = UIKeyboardType.PhonePad;
        //            break;

        //        case InputType.Url:
        //            txt.KeyboardType = UIKeyboardType.Url;
        //            break;
        //    }
        //}

        #endregion
    }
}
