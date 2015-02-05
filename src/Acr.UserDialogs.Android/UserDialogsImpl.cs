using System;
using System.Linq;
using Android.App;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using AndroidHUD;


namespace Acr.UserDialogs {

    public class UserDialogsImpl : AbstractUserDialogs {
        private readonly Func<Activity> getTopActivity;
 

        public UserDialogsImpl(Func<Activity> getTopActivity) {
            this.getTopActivity = getTopActivity;
        }


        public override void Alert(AlertConfig config) {
            Utils.RequestMainThread(() =>
                new AlertDialog
                    .Builder(this.getTopActivity())
                    .SetCancelable(false)
                    .SetMessage(config.Message)
                    .SetTitle(config.Title)
                    .SetPositiveButton(config.OkText, (o, e) => {
                        if (config.OnOk != null)
                            config.OnOk();
                    })
                    .Show()
            );
        }


        public override void ActionSheet(ActionSheetConfig config) {
            var array = config
                .Options
                .Select(x => x.Text)
                .ToArray();

            Utils.RequestMainThread(() => 
                new AlertDialog
                    .Builder(this.getTopActivity())
                    .SetCancelable(false)
                    .SetTitle(config.Title)
                    .SetItems(array, (sender, args) => config.Options[args.Which].Action())
                    .Show()
            );
        }


        public override void Confirm(ConfirmConfig config) {
            Utils.RequestMainThread(() => 
                new AlertDialog
                    .Builder(this.getTopActivity())
                    .SetCancelable(false)
                    .SetMessage(config.Message)
                    .SetTitle(config.Title)
                    .SetPositiveButton(config.OkText, (o, e) => config.OnConfirm(true))
                    .SetNegativeButton(config.CancelText, (o, e) => config.OnConfirm(false))
                    .Show()
            );
        }


        public override void Login(LoginConfig config) {
            var context = this.getTopActivity();
            var txtUser = new EditText(context) {
                Hint = config.LoginPlaceholder,
                InputType = InputTypes.TextVariationVisiblePassword,
                Text = config.LoginValue ?? String.Empty
            };
            var txtPass = new EditText(context) {
                Hint = config.PasswordPlaceholder ?? "*"
            };
            this.SetInputType(txtPass, InputType.Password);

            var layout = new LinearLayout(context) {
                Orientation = Orientation.Vertical
            };

            txtUser.SetMaxLines(1);
            txtPass.SetMaxLines(1);

            layout.AddView(txtUser, ViewGroup.LayoutParams.MatchParent);
            layout.AddView(txtPass, ViewGroup.LayoutParams.MatchParent);

            Utils.RequestMainThread(() => {
                var dialog = new AlertDialog
                    .Builder(this.getTopActivity())
                    .SetCancelable(false)
                    .SetTitle(config.Title)
                    .SetMessage(config.Message)
                    .SetView(layout)
                    .SetPositiveButton(config.OkText, (o, e) =>
                        config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, true))
                    )
                    .SetNegativeButton(config.CancelText, (o, e) =>
                        config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, false))
                    ).Create();
                dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
                dialog.Show();
            });
        }


        public override void Prompt(PromptConfig config) {
            Utils.RequestMainThread(() => {
                var activity = this.getTopActivity();

                var txt = new EditText(activity) {
                    Hint = config.Placeholder
                };
                if (config.InputType != InputType.Default) 
                    txt.SetMaxLines(1);

                this.SetInputType(txt, config.InputType);

                var dialog = new AlertDialog
                    .Builder(activity)
                    .SetCancelable(false)
                    .SetMessage(config.Message)
                    .SetTitle(config.Title)
                    .SetView(txt)
                    .SetPositiveButton(config.OkText, (o, e) =>
                        config.OnResult(new PromptResult {
                            Ok = true, 
                            Text = txt.Text
                        })
                    )
                    .SetNegativeButton(config.CancelText, (o, e) => 
                        config.OnResult(new PromptResult {
                            Ok = false, 
                            Text = txt.Text
                        })
                    ).Create();
                dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
                dialog.Show();
            });
        }


        public override void Toast(string message, int timeoutSeconds = 3, Action onClick = null) {
            Utils.RequestMainThread(() => {
                onClick = onClick ?? (() => {});

                AndHUD.Shared.ShowToast(
                    this.getTopActivity(),
                    message,
                    MaskType.Clear,
                    TimeSpan.FromSeconds(timeoutSeconds),
                    false,
                    onClick
                );
            });
        }


        protected override IProgressDialog CreateDialogInstance() {
            return new ProgressDialog();
        }


		protected override IProgressIndicator CreateNetworkIndicator() {
            return new NetworkIndicator(this.getTopActivity());
        }


        protected virtual void SetInputType(TextView txt, InputType inputType) {
            switch (inputType) {
                case InputType.Email:
                    txt.InputType = InputTypes.TextVariationEmailAddress;
                    break;

                case InputType.Number:
                    txt.InputType = InputTypes.ClassNumber;
                    break;

                case InputType.Password:
                    txt.TransformationMethod = PasswordTransformationMethod.Instance;
                    txt.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
                    break;
            }
        }
    }
}