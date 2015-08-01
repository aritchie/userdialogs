using System;
using System.Linq;
using Android.App;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using com.dbeattie;
using Splat;


namespace Acr.UserDialogs {

    public class UserDialogsImpl : AbstractUserDialogs {
        readonly Func<Activity> getTopActivity;
        readonly Func<IAlertDialog> dialogBuilder;


        public UserDialogsImpl(Func<Activity> getTopActivity, bool useMaterialDesign) {
            this.getTopActivity = getTopActivity ?? (() => ActivityLifecycleCallbacks.CurrentTopActivity);

            if (useMaterialDesign)
                this.dialogBuilder = () => new AppCompatAlertDialog(this.getTopActivity());
            else
                this.dialogBuilder = () => new StandardAlertDialog(this.getTopActivity());
        }


        public override void Alert(AlertConfig config) {
            //var context = this.getTopActivity();
            //var layout = new LinearLayout(context) {
            //    Orientation = Orientation.Vertical,
            //    OverScrollMode = OverScrollMode.IfContentScrolls
            //};
            //var txt = new TextView(context);

            Utils.RequestMainThread(() =>
                this.dialogBuilder()
                    .SetCancelable(false)
                    .SetMessage(config.Message)
                    .SetTitle(config.Title)
					.SetPositiveButton(config.OkText, () => config.OnOk?.Invoke())
                    .Show()
            );
        }


        public override void ActionSheet(ActionSheetConfig config) {
            var array = config
                .Options
                .Select(x => x.Text)
                .ToArray();

			var dlg = this.dialogBuilder()
				.SetCancelable(false)
				.SetTitle(config.Title);

            dlg.SetItems(array, index => config.Options[index].Action?.Invoke());

			if (config.Destructive != null)
				dlg.SetNegativeButton(config.Destructive.Text, () => config.Destructive.Action?.Invoke());

			if (config.Cancel != null)
				dlg.SetNeutralButton(config.Cancel.Text, () => config.Cancel.Action?.Invoke());

			Utils.RequestMainThread(() => dlg.Show());
        }


        public override void Confirm(ConfirmConfig config) {
            Utils.RequestMainThread(() =>
                this.dialogBuilder()
                    .SetCancelable(false)
                    .SetMessage(config.Message)
                    .SetTitle(config.Title)
                    .SetPositiveButton(config.OkText, () => config.OnConfirm(true))
                    .SetNegativeButton(config.CancelText, () => config.OnConfirm(false))
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

            Utils.RequestMainThread(() =>
                this.dialogBuilder()
                    .SetCancelable(false)
                    .SetTitle(config.Title)
                    .SetMessage(config.Message)
                    .SetView(layout)
                    .SetPositiveButton(config.OkText, () =>
                        config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, true))
                    )
                    .SetNegativeButton(config.CancelText, () =>
                        config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, false))
                    )
                    .Show()
            );
        }


        public override void Prompt(PromptConfig config) {
            Utils.RequestMainThread(() => {
                var activity = this.getTopActivity();

                var txt = new EditText(activity) {
                    Hint = config.Placeholder
                };
				if (config.Text != null)
					txt.Text = config.Text;

                this.SetInputType(txt, config.InputType);

                var builder = this.dialogBuilder()
                    .SetCancelable(false)
                    .SetMessage(config.Message)
                    .SetTitle(config.Title)
                    .SetView(txt)
                    .SetPositiveButton(config.OkText, () =>
                        config.OnResult(new PromptResult {
                            Ok = true,
                            Text = txt.Text
                        })
					);

				if (config.IsCancellable) {
					builder.SetNegativeButton(config.CancelText, () =>
                        config.OnResult(new PromptResult {
                            Ok = false,
                            Text = txt.Text
                        })
					);
				}

				builder.Show();
            });
        }


        class ToastListener : IActionClickListener {
            readonly Action onClick;
            public ToastListener(Action onClick) {
                this.onClick = onClick;
            }

            public void OnActionClicked(Snackbar snackbar) {
                this.onClick?.Invoke();
            }
        }


        public override void Toast(ToastConfig cfg) {
			var top = this.getTopActivity();
            var bar = Snackbar
                .With(top)
                .Duration((long)cfg.Duration.TotalMilliseconds)
                .DismissOnActionClicked(true)
                .Color(cfg.TextColor.ToNative())
                .Text(cfg.Text);

            bar.Background = new ColorDrawable(cfg.BackgroundColor.ToNative());
            if (cfg.Action != null)
                bar
                    .ActionLabel(cfg.ActionText)
                    .ActionColor(cfg.ActionTextColor.ToNative())
                    .ActionListener(new ToastListener(cfg.Action));

            //if (cfg.BackgroundColor != null)
            //    bar.Color(cfg.BackgroundColor.Value.ToNative());

            // TEXT COLOR - ACTION COLOR
            //Utils.RequestMainThread(() => bar.Show(top));
            //Utils.RequestMainThread(() => SnackbarManager.Show(bar));
            //var view = top.FindViewById(Android.Resource.Id.Content).RootView;
            //var view = top.Window.DecorView.RootView;
            //var view = top.Window.DecorView.FindViewById(Android.Resource.Id.Content);
            //Console.WriteLine("View is " + (view == null ? "NULL" : view.Id.ToString()));
            //var snackBar = Snackbar.Make(view, cfg.Message, (int)cfg.Duration.TotalMilliseconds);

            ////if (cfg.BackgroundColor != null)
            ////    snackBar.SetActionTextColor()

            //if (cfg.OnTap != null)
            //    snackBar.SetAction("Ok", x => cfg.OnTap?.Invoke());

            //Utils.RequestMainThread(snackBar.Show);
        }


        protected override IProgressDialog CreateDialogInstance() {
			return new ProgressDialog(this.getTopActivity());
        }


        protected virtual void SetInputType(TextView txt, InputType inputType) {
            switch (inputType) {

                case InputType.Email:
                    txt.InputType = InputTypes.ClassText | InputTypes.TextVariationEmailAddress;
                    txt.SetSingleLine(true);
                    break;

				case InputType.Name:
					txt.InputType = InputTypes.TextVariationPersonName;
                    txt.SetSingleLine(true);
					break;

                case InputType.Number:
                    txt.InputType = InputTypes.ClassNumber;
                    txt.SetSingleLine(true);
                    break;

                case InputType.NumericPassword:
                    txt.InputType = InputTypes.ClassNumber;
                    txt.TransformationMethod = PasswordTransformationMethod.Instance;
                    break;

                case InputType.Password:
                    txt.TransformationMethod = PasswordTransformationMethod.Instance;
                    txt.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
                    break;

				case InputType.Phone:
					txt.InputType = InputTypes.ClassPhone;
                    txt.SetSingleLine(true);
					break;

				case InputType.Url:
					txt.InputType = InputTypes.TextVariationUri;
                    txt.SetSingleLine(true);
					break;
            }
        }
    }
}