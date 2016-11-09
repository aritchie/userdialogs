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
                //ActionSheetConfig.DefaultCancelText = LoginConfig.DefaultNeutralText = PromptConfig.DefaultNeutralText = ProgressDialogConfig.DefaultCancelText = "Cancel";

                // OK
                //AlertConfig.DefaultPositiveText = LoginConfig.DefaultPositiveText = PromptConfig.DefaultPositiveText = "Ok";

                // CUSTOM
                ActionSheetConfig.DefaultDestructiveText = "Remove";
                DatePromptConfig.DefaultNeutral.Text = "Cancel";
                DatePromptConfig.DefaultPositive.Text = "Ok";
                TimePromptConfig.DefaultMinuteInterval = 1;
                TimePromptConfig.DefaultNeutral.Text = "Cancel";
                TimePromptConfig.DefaultPositive.Text = "Ok";
                LoginConfig.DefaultTitle = "Login";
                LoginConfig.DefaultLoginPlaceholder = "User Name";
                LoginConfig.DefaultPasswordPlaceholder = "Password";
                ProgressDialogConfig.DefaultTitle = "Loading";

                ToastConfig.DefaultDuration = TimeSpan.FromSeconds(3);

                this.Result("Default Settings Loading - Now run samples");
            });

            this.LoadAbnormalSettings = new Command(() =>
            {
                //ActionSheetConfig.DefaultCancelText = LoginConfig.DefaultNeutralText = PromptConfig.DefaultNeutralText = ProgressDialogConfig.DefaultCancelText = "NO WAY";

                //// OK
                //AlertConfig.DefaultPositiveText = LoginConfig.DefaultPositiveText = PromptConfig.DefaultPositiveText = "Sure";

                //// CUSTOM
                //ActionSheetConfig.DefaultDestructiveText = "BOOM!";

                //DatePromptConfig.DefaultCancelText = "BYE";
                //DatePromptConfig.DefaultOkText = "Do Something";

                //TimePromptConfig.DefaultMinuteInterval = 15;
                //TimePromptConfig.DefaultCancelText = "BYE";
                //TimePromptConfig.DefaultOkText = "Do Something";
                //LoginConfig.DefaultTitle = "HIGH SECURITY";
                //LoginConfig.DefaultLoginPlaceholder = "WHO ARE YOU?";
                //LoginConfig.DefaultPasswordPlaceholder = "SUPER SECRET PASSWORD";
                //ProgressDialogConfig.DefaultTitle = "WAIT A MINUTE";

                // TOAST
                ToastConfig.DefaultDuration = TimeSpan.FromSeconds(5);

                this.Result("Abnormal Settings Loaded - Now run samples");
            });
        }


        public ICommand StandardSettings { get; }
        public ICommand LoadAbnormalSettings { get; }
    }
}
