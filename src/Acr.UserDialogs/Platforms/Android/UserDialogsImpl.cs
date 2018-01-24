using System;
using Acr.UserDialogs.Builders;
using Acr.UserDialogs.Fragments;
using Acr.UserDialogs.Infrastructure;
using Android.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Text.Style;
using AndroidHUD;


namespace Acr.UserDialogs
{
    public class UserDialogsImpl : AbstractUserDialogs
    {
        public static string FragmentTag { get; set; } = "UserDialogs";
        protected internal Func<Activity> TopActivityFunc { get; set; }


        public UserDialogsImpl(Func<Activity> getTopActivity)
        {
            this.TopActivityFunc = getTopActivity;
        }


        #region Alert Dialogs

        public override IDisposable Alert(AlertConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity act)
                return this.ShowDialog<AlertAppCompatDialogFragment, AlertConfig>(act, config);

            return this.Show(activity, () => new AlertBuilder().Build(activity, config));
        }


        public override IDisposable ActionSheet(ActionSheetConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity act)
            {
                if (config.UseBottomSheet)
                    return this.ShowDialog<Fragments.BottomSheetDialogFragment, ActionSheetConfig>(act, config);

                return this.ShowDialog<ActionSheetAppCompatDialogFragment, ActionSheetConfig>(act, config);
            }

            return this.Show(activity, () => new ActionSheetBuilder().Build(activity, config));
        }


        public override IDisposable Confirm(ConfirmConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity act)
                return this.ShowDialog<ConfirmAppCompatDialogFragment, ConfirmConfig>(act, config);

            return this.Show(activity, () => new ConfirmBuilder().Build(activity, config));
        }


        public override IDisposable DatePrompt(DatePromptConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity act)
                return this.ShowDialog<DateAppCompatDialogFragment, DatePromptConfig>(act, config);

            return this.Show(activity, () => DatePromptBuilder.Build(activity, config));
        }


        public override IDisposable Login(LoginConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity act)
                return this.ShowDialog<LoginAppCompatDialogFragment, LoginConfig>(act, config);

            return this.Show(activity, () => new LoginBuilder().Build(activity, config));
        }


        public override IDisposable Prompt(PromptConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity act)
                return this.ShowDialog<PromptAppCompatDialogFragment, PromptConfig>(act, config);

            return this.Show(activity, () => new PromptBuilder().Build(activity, config));
        }


        public override IDisposable TimePrompt(TimePromptConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity act)
                return this.ShowDialog<TimeAppCompatDialogFragment, TimePromptConfig>(act, config);

            return this.Show(activity, () => TimePromptBuilder.Build(activity, config));
        }

        #endregion

        #region Toasts

        public override IDisposable Toast(ToastConfig cfg)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity compat)
                return this.ToastAppCompat(compat, cfg);

            return this.ToastFallback(activity, cfg);
        }


        protected virtual IDisposable ToastAppCompat(AppCompatActivity activity, ToastConfig cfg)
        {
            Snackbar snackBar = null;
            activity.SafeRunOnUi(() =>
            {
                var view = activity.Window.DecorView.RootView.FindViewById(Android.Resource.Id.Content);
                var msg = this.GetSnackbarText(cfg);

                snackBar = Snackbar.Make(
                    view,
                    msg,
                    (int)cfg.Duration.TotalMilliseconds
                );
                if (cfg.BackgroundColor != null)
                    snackBar.View.SetBackgroundColor(cfg.BackgroundColor.Value.ToNative());

                if (cfg.Position == ToastPosition.Top)
                {
                    // watch for this to change in future support lib versions
                    var layoutParams = snackBar.View.LayoutParameters as FrameLayout.LayoutParams;
                    if (layoutParams != null)
                    {
                        layoutParams.Gravity = GravityFlags.Top;
                        layoutParams.SetMargins(0, 80, 0, 0);
                        snackBar.View.LayoutParameters = layoutParams;
                    }
                }
                if (cfg.Action != null)
                {
                    snackBar.SetAction(cfg.Action.Text, x =>
                    {
                        cfg.Action?.Action?.Invoke();
                        snackBar.Dismiss();
                    });
                    var color = cfg.Action.TextColor;
                    if (color != null)
                        snackBar.SetActionTextColor(color.Value.ToNative());
                }

                snackBar.Show();
            });
            return new DisposableAction(() =>
            {
                if (snackBar.IsShown)
                    activity.SafeRunOnUi(snackBar.Dismiss);
            });
        }


        protected virtual ISpanned GetSnackbarText(ToastConfig cfg)
        {
            var sb = new SpannableStringBuilder();

            string message = cfg.Message;
            var hasIcon = (cfg.Icon != null);
            if (hasIcon)
                message = "\u2002\u2002" + message; // add 2 spaces, 1 for the image the next for spacing between text and image

            sb.Append(message);

            if (hasIcon)
            {
                var drawable = ImageLoader.Load(cfg.Icon);
                drawable.SetBounds(0, 0, drawable.IntrinsicWidth, drawable.IntrinsicHeight);

                sb.SetSpan(new ImageSpan(drawable, SpanAlign.Bottom), 0, 1, SpanTypes.ExclusiveExclusive);
            }

            if (cfg.MessageTextColor != null)
            {
                sb.SetSpan(
                    new ForegroundColorSpan(cfg.MessageTextColor.Value.ToNative()),
                    0,
                    sb.Length(),
                    SpanTypes.ExclusiveExclusive
                );
            }
            return sb;
        }


        protected virtual string ToHex(System.Drawing.Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            //var alpha = (int)(color.A * 255);
            //var hex = String.Format($"#{red:X2}{green:X2}{blue:X2}{alpha:X2}");
            var hex = String.Format($"#{red:X2}{green:X2}{blue:X2}");
            return hex;
        }


        protected virtual IDisposable ToastFallback(Activity activity, ToastConfig cfg)
        {
            AndHUD.Shared.ShowToast(
                activity,
                cfg.Message,
                AndroidHUD.MaskType.None,
                cfg.Duration,
                false,
                () =>
                {
                    AndHUD.Shared.Dismiss();
                    cfg.Action?.Action?.Invoke();
                }
            );
            return new DisposableAction(() =>
            {
                try
                {
                    AndHUD.Shared.Dismiss(activity);
                }
                catch
                {
                }
            });
        }

        #endregion

        #region Internals

        protected override IProgressDialog CreateDialogInstance(ProgressDialogConfig config)
        {
            var activity = this.TopActivityFunc();
            var dialog = new ProgressDialog(config, activity);


            //if (activity != null)
            //{
            //    var frag = new LoadingFragment();
            //    activity.RunOnUiThread(() =>
            //    {
            //        frag.Config = dialog;
            //        frag.Show(activity.SupportFragmentManager, FragmentTag);
            //    });
            //}

            return dialog;
        }


        protected virtual IDisposable Show(Activity activity, Func<Dialog> dialogBuilder)
        {
            Dialog dialog = null;
            activity.SafeRunOnUi(() =>
            {
                dialog = dialogBuilder();
                dialog.Show();
            });
            return new DisposableAction(() =>
                activity.SafeRunOnUi(dialog.Dismiss)
            );
        }


        protected virtual IDisposable ShowDialog<TFragment, TConfig>(AppCompatActivity activity, TConfig config) where TFragment : AbstractAppCompatDialogFragment<TConfig> where TConfig : class, new()
        {
            var frag = (TFragment)Activator.CreateInstance(typeof(TFragment));
            activity.SafeRunOnUi(() =>
            {
                frag.Config = config;
                frag.Show(activity.SupportFragmentManager, FragmentTag);
            });
            return new DisposableAction(() =>
                activity.SafeRunOnUi(frag.Dismiss)
            );
        }

        #endregion
    }
}