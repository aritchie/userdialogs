using System;
using System.Windows.Input;
using Acr.UserDialogs;
using Xamarin.Forms;


namespace Samples.ViewModels
{
    public class SettingsViewModel : AbstractViewModel
    {
        public SettingsViewModel(IUserDialogs dialogs) : base(dialogs)
        {
            this.StandardSettings = new Command(() =>
            {
                // CANCEL
                ActionSheetConfig.DefaultCancelText = ConfirmConfig.DefaultCancelText = LoginConfig.DefaultCancelText = PromptConfig.DefaultCancelText = ProgressDialogConfig.DefaultCancelText = "Cancel";

                // OK
                AlertConfig.DefaultOkText = ConfirmConfig.DefaultOkText = LoginConfig.DefaultOkText = PromptConfig.DefaultOkText = "Ok";

                // CUSTOM
                ActionSheetConfig.DefaultDestructiveText = "Remove";
                ConfirmConfig.DefaultYes = "Yes";
                ConfirmConfig.DefaultNo = "No";
                DatePromptConfig.DefaultCancelText = "Cancel";
                DatePromptConfig.DefaultOkText = "Ok";
                TimePromptConfig.DefaultMinuteInterval = 1;
                TimePromptConfig.DefaultCancelText = "Cancel";
                TimePromptConfig.DefaultOkText = "Ok";
                LoginConfig.DefaultTitle = "Login";
                LoginConfig.DefaultLoginPlaceholder = "User Name";
                LoginConfig.DefaultPasswordPlaceholder = "Password";
                ProgressDialogConfig.DefaultTitle = "Loading";

                ToastConfig.DefaultDuration = TimeSpan.FromSeconds(3);

                this.Result("Default Settings Loading - Now run samples");
            });

            this.LoadAbnormalSettings = new Command(() =>
            {
                ActionSheetConfig.DefaultCancelText = ConfirmConfig.DefaultCancelText = LoginConfig.DefaultCancelText = PromptConfig.DefaultCancelText = ProgressDialogConfig.DefaultCancelText = "NO WAY";

                // OK
                AlertConfig.DefaultOkText = ConfirmConfig.DefaultOkText = LoginConfig.DefaultOkText = PromptConfig.DefaultOkText = "Sure";

                // CUSTOM
                ActionSheetConfig.DefaultDestructiveText = "BOOM!";
                ConfirmConfig.DefaultYes = "SIGN LIFE AWAY";
                ConfirmConfig.DefaultNo = "NO WAY";

                DatePromptConfig.DefaultCancelText = "BYE";
                DatePromptConfig.DefaultOkText = "Do Something";

                TimePromptConfig.DefaultMinuteInterval = 15;
                TimePromptConfig.DefaultCancelText = "BYE";
                TimePromptConfig.DefaultOkText = "Do Something";
                LoginConfig.DefaultTitle = "HIGH SECURITY";
                LoginConfig.DefaultLoginPlaceholder = "WHO ARE YOU?";
                LoginConfig.DefaultPasswordPlaceholder = "SUPER SECRET PASSWORD";
                ProgressDialogConfig.DefaultTitle = "WAIT A MINUTE";

                // TOAST
                ToastConfig.DefaultDuration = TimeSpan.FromSeconds(5);

                //ToastConfig.InfoBackgroundColor = System.Drawing.Color.Aqua;
                //ToastConfig.SuccessTextColor = System.Drawing.Color.Blue;
                //ToastConfig.SuccessBackgroundColor = System.Drawing.Color.BurlyWood;
                //ToastConfig.WarnBackgroundColor = System.Drawing.Color.BlueViolet;
                //ToastConfig.ErrorBackgroundColor = System.Drawing.Color.DeepPink;

                this.Result("Abnormal Settings Loaded - Now run samples");
            });
        }


        public ICommand StandardSettings { get; }
        public ICommand LoadAbnormalSettings { get; }
    }
}
