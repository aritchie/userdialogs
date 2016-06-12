using System;
using Acr.UserDialogs.Builders;
using Acr.UserDialogs.Fragments;
using Android.App;
using Android.Views;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Text;
using Android.Widget;
using AndroidHUD;
using Splat;


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
            if (activity is AppCompatActivity)
                return this.ShowDialog<AlertAppCompatDialogFragment, AlertConfig>((AppCompatActivity)activity, config);

            if (activity is FragmentActivity)
                return this.ShowDialog<AlertDialogFragment, AlertConfig>((FragmentActivity)activity, config);

            return this.Show(activity, new AlertBuilder().Build(activity, config).Create());
        }


        public override IDisposable ActionSheet(ActionSheetConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity)
                return this.ShowDialog<Fragments.BottomSheetDialogFragment, ActionSheetConfig>((AppCompatActivity)activity, config);
            //return this.ShowDialog<ActionSheetAppCompatDialogFragment, ActionSheetConfig>((AppCompatActivity)activity, config);

            if (activity is FragmentActivity)
                return this.ShowDialog<ActionSheetDialogFragment, ActionSheetConfig>((FragmentActivity)activity, config);

            return this.Show(activity, new ActionSheetBuilder().Build(activity, config).Create());
        }


        public override IDisposable Confirm(ConfirmConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity)
                return this.ShowDialog<ConfirmAppCompatDialogFragment, ConfirmConfig>((AppCompatActivity)activity, config);

            if (activity is FragmentActivity)
                return this.ShowDialog<ConfirmDialogFragment, ConfirmConfig>((FragmentActivity)activity, config);

            return this.Show(activity, new ConfirmBuilder().Build(activity, config).Create());
        }


        public override IDisposable DatePrompt(DatePromptConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity)
                return this.ShowDialog<DateAppCompatDialogFragment, DatePromptConfig>((AppCompatActivity)activity, config);

            if (activity is FragmentActivity)
                return this.ShowDialog<DateDialogFragment, DatePromptConfig>((FragmentActivity)activity, config);

            return this.Show(activity, DatePromptBuilder.Build(activity, config));
        }


        public override IDisposable Login(LoginConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity)
                return this.ShowDialog<LoginAppCompatDialogFragment, LoginConfig>((AppCompatActivity)activity, config);

            if (activity is FragmentActivity)
                return this.ShowDialog<LoginDialogFragment, LoginConfig>((FragmentActivity)activity, config);

            return this.Show(activity, new LoginBuilder().Build(activity, config).Create());
        }


        public override IDisposable Prompt(PromptConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity)
                return this.ShowDialog<PromptAppCompatDialogFragment, PromptConfig>((AppCompatActivity)activity, config);

            if (activity is FragmentActivity)
                return this.ShowDialog<PromptDialogFragment, PromptConfig>((FragmentActivity)activity, config);

            return this.Show(activity, new PromptBuilder().Build(activity, config).Create());
        }


        public override IDisposable TimePrompt(TimePromptConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity)
                return this.ShowDialog<TimeAppCompatDialogFragment, TimePromptConfig>((AppCompatActivity)activity, config);

            if (activity is FragmentActivity)
                return this.ShowDialog<TimeDialogFragment, TimePromptConfig>((FragmentActivity)activity, config);

            return this.Show(activity, TimePromptBuilder.Build(activity, config));
        }

        #endregion

        #region Images

        public override void ShowImage(IBitmap image, string message, int timeoutMillis)
        {
            var activity = this.TopActivityFunc();
            activity.RunOnUiThread(() =>
                AndHUD.Shared.ShowImage(activity, image.ToNative(), message, AndroidHUD.MaskType.Black, TimeSpan.FromMilliseconds(timeoutMillis))
            );
        }


        public override void ShowSuccess(string message, int timeoutMillis)
        {
            var activity = this.TopActivityFunc();
            activity.RunOnUiThread(() =>
                AndHUD.Shared.ShowSuccess(activity, message, timeout: TimeSpan.FromMilliseconds(timeoutMillis))
            );
        }


        public override void ShowError(string message, int timeoutMillis)
        {
            var activity = this.TopActivityFunc();
            activity.RunOnUiThread(() =>
                AndHUD.Shared.ShowError(activity, message, timeout: TimeSpan.FromMilliseconds(timeoutMillis))
            );
        }

        #endregion

        #region Toasts

        public override IDisposable Toast(ToastConfig cfg)
        {
            var activity = this.TopActivityFunc();
            var compat = activity as AppCompatActivity;

            if (compat == null)
                return this.ToastFallback(activity, cfg);

            return this.ToastAppCompat(compat, cfg);
        }


        protected virtual IDisposable ToastAppCompat(AppCompatActivity activity, ToastConfig cfg)
        {
            var view = activity.Window.DecorView.RootView.FindViewById(Android.Resource.Id.Content);
            var snackBar = Snackbar.Make(
                view,
                Html.FromHtml(cfg.Message),
                (int)cfg.Duration.TotalMilliseconds
            );

            if (cfg.PrimaryAction != null)
            {
                snackBar.SetAction(cfg.PrimaryAction.Text, x =>
                {
                    cfg.PrimaryAction.Action?.Invoke();
                    snackBar.Dismiss();
                });
                var color = cfg.PrimaryAction.TextColor ?? ToastConfig.DefaultPrimaryTextColor;
                snackBar.SetActionTextColor(color.ToNative());
            }

            activity.RunOnUiThread(snackBar.Show);
            return new DisposableAction(() =>
            {
                if (snackBar.IsShown)
                    activity.RunOnUiThread(() =>
                    {
                        try
                        {
                            snackBar.Dismiss();
                        }
                        catch
                        {
                            // catch and swallow
                        }
                    });
            });
        }


        protected virtual IDisposable ToastFallback(Activity activity, ToastConfig cfg)
        {
            activity.RunOnUiThread(() =>
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
                        cfg.PrimaryAction.Action?.Invoke();
                    }
                );
            });
            return new DisposableAction(() =>
                activity.RunOnUiThread(() =>
                    AndHUD.Shared.Dismiss(activity)
                )
            );
        }

        #endregion

            #region Internals

        protected override IProgressDialog CreateDialogInstance()
        {
            return new ProgressDialog(this.TopActivityFunc());
        }


        protected virtual IDisposable Show(Activity activity, Dialog dialog)
        {
            activity.RunOnUiThread(dialog.Show);
            return new DisposableAction(() =>
                activity.RunOnUiThread(dialog.Dismiss)
            );
        }


        protected virtual IDisposable Show(Activity activity, Android.App.AlertDialog.Builder builder)
        {
            var dialog = builder.Show();
            dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            activity.RunOnUiThread(dialog.Show);
            return new DisposableAction(() =>
                activity.RunOnUiThread(dialog.Dismiss)
            );
        }


        protected virtual IDisposable ShowDialog<TFragment, TConfig>(FragmentActivity activity, TConfig config) where TFragment : AbstractDialogFragment<TConfig> where TConfig : class, new()
        {
            var frag = (TFragment)Activator.CreateInstance(typeof(TFragment));

            activity.RunOnUiThread(() =>
            {
                frag.Config = config;
                frag.Show(activity.FragmentManager, FragmentTag);
            });
            return new DisposableAction(() =>
                activity.RunOnUiThread(frag.Dismiss)
            );
        }


        protected virtual IDisposable ShowDialog<TFragment, TConfig>(AppCompatActivity activity, TConfig config) where TFragment : AbstractAppCompatDialogFragment<TConfig> where TConfig : class, new()
        {
            var frag = (TFragment)Activator.CreateInstance(typeof(TFragment));
            activity.RunOnUiThread(() =>
            {
                frag.Config = config;
                frag.Show(activity.SupportFragmentManager, FragmentTag);
            });
            return new DisposableAction(() =>
                activity.RunOnUiThread(frag.Dismiss)
            );
        }

        #endregion
    }
}