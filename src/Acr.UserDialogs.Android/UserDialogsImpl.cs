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
					.SetPositiveButton(config.OkText, (o, e) => config.OnOk.TryExecute())
                    .Show()
            );
        }


        public override void ActionSheet(ActionSheetConfig config) {
            var array = config
                .Options
                .Select(x => x.Text)
                .ToArray();

			var dlg = new AlertDialog
				.Builder(this.getTopActivity())
				.SetCancelable(false)
				.SetTitle(config.Title);

			dlg.SetItems(array, (sender, args) => config.Options[args.Which].Action.TryExecute());

			if (config.Destructive != null)
				dlg.SetNegativeButton(config.Destructive.Text, (sender, e) => config.Destructive.Action.TryExecute());

			if (config.Cancel != null)
				dlg.SetNeutralButton(config.Cancel.Text, (sender, e) => config.Cancel.Action.TryExecute());

			Utils.RequestMainThread(() => dlg.Show());
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
                    )
					.Create();

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
				if (config.Text != null)
					txt.Text = config.Text;

                if (config.InputType != InputType.Default) 
                    txt.SetMaxLines(1);

                this.SetInputType(txt, config.InputType);

                var builder = new AlertDialog
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
					);

				if (config.IsCancellable) {
					builder.SetNegativeButton(config.CancelText, (o, e) => 
                        config.OnResult(new PromptResult {
                            Ok = false, 
                            Text = txt.Text
                        })
					);
				}

				var dialog = builder.Create();
                dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
                dialog.Show();
            });
        }


        public override void Toast(string message, int timeoutSeconds = 3, Action onClick = null) {
            Utils.RequestMainThread(() => {

				var top = this.getTopActivity();
                AndHUD.Shared.ShowToast(
                    top,
                    message,
					MaskType.Clear,
                    TimeSpan.FromSeconds(timeoutSeconds),
                    false,
					() => {
						AndHUD.Shared.Dismiss();
						if (onClick != null)
							onClick();
					}
                );
            });
        }


        protected override IProgressDialog CreateDialogInstance() {
			return new ProgressDialog(this.getTopActivity());
        }


        protected virtual void SetInputType(TextView txt, InputType inputType) {
            switch (inputType) {

                case InputType.Email:
                    txt.InputType = InputTypes.ClassText | InputTypes.TextVariationEmailAddress;
                    break;

				case InputType.Name:
					txt.InputType = InputTypes.TextVariationPersonName;
					break;

                case InputType.Number:
                    txt.InputType = InputTypes.ClassNumber;
                    break;

                case InputType.Password:
                    txt.TransformationMethod = PasswordTransformationMethod.Instance;
                    txt.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
                    break;

				case InputType.Phone:
					txt.InputType = InputTypes.ClassPhone;
					break;

				case InputType.Url:
					txt.InputType = InputTypes.TextVariationUri;
					break;
            }
        }
    }
}