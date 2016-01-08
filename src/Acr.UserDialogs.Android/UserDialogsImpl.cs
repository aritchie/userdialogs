using System;
using System.Linq;
using Android.App;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Splat;

#if APPCOMPAT
using System.Collections.Generic;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Support.Design.Widget;
using AlertDialog = Android.Support.V7.App.AlertDialog;
#else
using AlertDialog = Android.App.AlertDialog;
#endif
using Utils = Acr.Support.Android.Extensions;


namespace Acr.UserDialogs {

    public class UserDialogsImpl : AbstractUserDialogs {
        protected Func<Activity> GetTopActivity { get; set; }


        public UserDialogsImpl(Func<Activity> getTopActivity) {
            this.GetTopActivity = getTopActivity;
        }


        public override void Alert(AlertConfig config) {
            //var context = this.GetTopActivity();
            //var layout = new LinearLayout(context) {
            //    Orientation = Orientation.Vertical,
            //    OverScrollMode = OverScrollMode.IfContentScrolls
            //};
            //var txt = new TextView(context);

            Utils.RequestMainThread(() =>
                new AlertDialog
                    .Builder(this.GetTopActivity())
                    .SetCancelable(false)
                    .SetMessage(config.Message)
                    .SetTitle(config.Title)
					.SetPositiveButton(config.OkText, (o, e) => config.OnOk?.Invoke())
                    .ShowExt()
            );
        }


        public override void ActionSheet(ActionSheetConfig config) {
            var activity = this.GetTopActivity();
            var dlg = new AlertDialog
                .Builder(activity)
				.SetCancelable(false)
                .SetTitle(config.Title);
                //.SetCustomTitle(new TextView(activity) {
                //    Text = config.Title,
                //    TextSize = 18.0f
                //});

            if (config.ItemIcon != null || config.Options.Any(x => x.ItemIcon != null)) {
                var adapter = new ActionSheetListAdapter(this.GetTopActivity(), Android.Resource.Layout.SelectDialogItem, Android.Resource.Id.Text1, config);
                dlg.SetAdapter(adapter, (s, a) => config.Options[a.Which].Action?.Invoke());
            }
            else {
                var array = config
                    .Options
                    .Select(x => x.Text)
                    .ToArray();

                dlg.SetItems(array, (s, args) => config.Options[args.Which].Action?.Invoke());
            }

			if (config.Destructive != null)
				dlg.SetNegativeButton(config.Destructive.Text, (s, a) => config.Destructive.Action?.Invoke());

			if (config.Cancel != null)
				dlg.SetNeutralButton(config.Cancel.Text, (s, a) => config.Cancel.Action?.Invoke());

			Utils.RequestMainThread(() => dlg.ShowExt());
        }


        public override void Confirm(ConfirmConfig config) {
            Utils.RequestMainThread(() =>
                new AlertDialog
                    .Builder(this.GetTopActivity())
                    .SetCancelable(false)
                    .SetMessage(config.Message)
                    .SetTitle(config.Title)
                    .SetPositiveButton(config.OkText, (s, a) => config.OnConfirm(true))
                    .SetNegativeButton(config.CancelText, (s, a) => config.OnConfirm(false))
                    .ShowExt()
            );
        }


        public override void Login(LoginConfig config) {
            var context = this.GetTopActivity();
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
                new AlertDialog
                    .Builder(context)
                    .SetCancelable(false)
                    .SetTitle(config.Title)
                    .SetMessage(config.Message)
                    .SetView(layout)
                    .SetPositiveButton(config.OkText, (s, a) =>
                        config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, true))
                    )
                    .SetNegativeButton(config.CancelText, (s, a) =>
                        config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, false))
                    )
                    .ShowExt()
            );
        }


        public override void Prompt(PromptConfig config) {
            Utils.RequestMainThread(() => {
                var activity = this.GetTopActivity();

                var txt = new EditText(activity) {
                    Hint = config.Placeholder
                };
				if (config.Text != null)
					txt.Text = config.Text;

                this.SetInputType(txt, config.InputType);

                var builder = new AlertDialog
                    .Builder(activity)
                    .SetCancelable(false)
                    .SetMessage(config.Message)
                    .SetTitle(config.Title)
                    .SetView(txt)
                    .SetPositiveButton(config.OkText, (s, a) =>
                        config.OnResult(new PromptResult {
                            Ok = true,
                            Text = txt.Text
                        })
					);

				if (config.IsCancellable) {
					builder.SetNegativeButton(config.CancelText, (s, a) =>
                        config.OnResult(new PromptResult {
                            Ok = false,
                            Text = txt.Text
                        })
					);
				}

				builder.ShowExt();
            });
        }


        public override void ShowImage(IBitmap image, string message, int timeoutMillis) {
            Utils.RequestMainThread(() =>
                AndHUD.Shared.ShowImage(this.GetTopActivity(), image.ToNative(), message, AndroidHUD.MaskType.Black, TimeSpan.FromMilliseconds(timeoutMillis))
            );
        }


        public override void ShowSuccess(string message, int timeoutMillis) {
            Utils.RequestMainThread(() =>
                AndHUD.Shared.ShowSuccess(this.GetTopActivity(), message, timeout: TimeSpan.FromMilliseconds(timeoutMillis))
            );
        }


        public override void ShowError(string message, int timeoutMillis) {
            Utils.RequestMainThread(() =>
                AndHUD.Shared.ShowError(this.GetTopActivity(), message, timeout: TimeSpan.FromMilliseconds(timeoutMillis))
            );
        }

#if APPCOMPAT

        public override void Toast(ToastConfig cfg) {
            var top = this.GetTopActivity();
            //var view = top.Window.DecorView.RootView;
            var view = top.Window.DecorView.RootView.FindViewById(Android.Resource.Id.Content);

            var text = $"<b>{cfg.Title}</b>";
            if (!String.IsNullOrWhiteSpace(cfg.Description))
                text += $"\n<br /><i>{cfg.Description}</i>";

            var snackBar = Snackbar.Make(view, text, (int)cfg.Duration.TotalMilliseconds);
            snackBar.View.Background = new ColorDrawable(cfg.BackgroundColor.ToNative());
            var txt = FindTextView(snackBar);
            txt.SetTextColor(cfg.TextColor.ToNative());
            txt.TextFormatted = Html.FromHtml(text);

            snackBar.View.Click += (sender, args) => {
                snackBar.Dismiss();
                cfg.Action?.Invoke();
            };
            Utils.RequestMainThread(snackBar.Show);
        }


        protected static TextView FindTextView(Snackbar bar) {
            var group = (ViewGroup)bar.View;
            for (var i = 0; i < group.ChildCount; i++) {
                var txt = group.GetChildAt(i) as TextView;
                if (txt != null)
                    return txt;
            }
            throw new Exception("No textview found on snackbar");
        }
#else
        public override void Toast(ToastConfig cfg) {
            Utils.RequestMainThread(() => {
				var top = this.GetTopActivity();
                var txt = cfg.Title;
                if (!String.IsNullOrWhiteSpace(cfg.Description))
                    txt += Environment.NewLine + cfg.Description;

                AndHUD.Shared.ShowToast(
                    top,
                    txt,
					AndroidHUD.MaskType.Black,
                    cfg.Duration,
                    false,
					() => {
						AndHUD.Shared.Dismiss();
                        cfg.Action?.Invoke();
					}
                );
            });
        }
#endif

        protected override IProgressDialog CreateDialogInstance() {
			return new ProgressDialog(this.GetTopActivity());
        }


        protected virtual void SetInputType(TextView txt, InputType inputType) {
            switch (inputType) {
                case InputType.DecimalNumber:
                    txt.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;
                    txt.SetSingleLine(true);
                    break;

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