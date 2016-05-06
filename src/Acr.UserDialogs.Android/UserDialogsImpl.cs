using System;
using Acr.UserDialogs.Builders;
using Acr.UserDialogs.Fragments;
using Android.App;
using Android.Views;
using Android.Support.V7.App;
using Android.Graphics.Drawables;
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

        public override void Alert(AlertConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity)
                this.ShowDialog<AlertAppCompatDialogFragment, AlertConfig>((AppCompatActivity)activity, config);

            else if (activity is FragmentActivity)
                this.ShowDialog<AlertDialogFragment, AlertConfig>((FragmentActivity)activity, config);

            else
                this.Show(activity, AlertBuilder.Build(activity, config));
        }


        public override void ActionSheet(ActionSheetConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity)
                this.ShowDialog<ActionSheetAppCompatDialogFragment, ActionSheetConfig>((AppCompatActivity)activity, config);

            else if (activity is FragmentActivity)
                this.ShowDialog<ActionSheetDialogFragment, ActionSheetConfig>((FragmentActivity)activity, config);

            else
                this.Show(activity, ActionSheetBuilder.Build(activity, config));
        }


        public override void Confirm(ConfirmConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity)
                this.ShowDialog<ConfirmAppCompatDialogFragment, ConfirmConfig>((AppCompatActivity)activity, config);

            else if (activity is FragmentActivity)
                this.ShowDialog<ConfirmDialogFragment, ConfirmConfig>((FragmentActivity)activity, config);

            else
                this.Show(activity, ConfirmBuilder.Build(activity, config));
        }


        public override void DatePrompt(DatePromptConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity)
                this.ShowDialog<DateAppCompatDialogFragment, DatePromptConfig>((AppCompatActivity)activity, config);

            else if (activity is FragmentActivity)
                this.ShowDialog<DateDialogFragment, DatePromptConfig>((FragmentActivity)activity, config);

            else
                this.Show(activity, DatePromptBuilder.Build(activity, config));
        }


        public override void Login(LoginConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity)
                this.ShowDialog<LoginAppCompatDialogFragment, LoginConfig>((AppCompatActivity)activity, config);

            else if (activity is FragmentActivity)
                this.ShowDialog<LoginDialogFragment, LoginConfig>((FragmentActivity)activity, config);

            else
                this.Show(activity, LoginBuilder.Build(activity, config));
        }


        public override void Prompt(PromptConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity)
                this.ShowDialog<PromptAppCompatDialogFragment, PromptConfig>((AppCompatActivity)activity, config);

            else if (activity is FragmentActivity)
                this.ShowDialog<PromptDialogFragment, PromptConfig>((FragmentActivity)activity, config);

            else
                this.Show(activity, PromptBuilder.Build(activity, config));
        }


        public override void TimePrompt(TimePromptConfig config)
        {
            var activity = this.TopActivityFunc();
            if (activity is AppCompatActivity)
                this.ShowDialog<TimeAppCompatDialogFragment, TimePromptConfig>((AppCompatActivity)activity, config);

            else if (activity is FragmentActivity)
                this.ShowDialog<TimeDialogFragment, TimePromptConfig>((FragmentActivity)activity, config);

            else
                this.Show(activity, TimePromptBuilder.Build(activity, config));
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

        public override void Toast(ToastConfig cfg)
        {
            var activity = this.TopActivityFunc();
            var compat = activity as AppCompatActivity;

            if (compat == null)
                this.ToastFallback(activity, cfg);
            else
                this.ToastAppCompat(compat, cfg);
        }


        protected virtual void ToastAppCompat(AppCompatActivity activity, ToastConfig cfg)
        {
            //var view = top.Window.DecorView.RootView;
            var view = activity.Window.DecorView.RootView.FindViewById(Android.Resource.Id.Content);
            var snackBar = Snackbar.Make(view, "TODO", (int)cfg.Duration.TotalMilliseconds);
            snackBar.View.Background = new ColorDrawable(cfg.BackgroundColor.ToNative());
            snackBar.View.Click += (sender, args) =>
            {
                snackBar.Dismiss();
                cfg.Action?.Invoke();
            };
            this.SetSnackbarTextView(snackBar, cfg);
            activity.RunOnUiThread(snackBar.Show);
        }


        protected virtual void ToastFallback(Activity activity, ToastConfig cfg)
        {
            activity.RunOnUiThread(() =>
            {
                var txt = cfg.Title;
                if (!String.IsNullOrWhiteSpace(cfg.Description))
                    txt += Environment.NewLine + cfg.Description;

                AndHUD.Shared.ShowToast(
                    activity,
                    txt,
                    AndroidHUD.MaskType.None,
                    cfg.Duration,
                    false,
                    () =>
                    {
                        AndHUD.Shared.Dismiss();
                        cfg.Action?.Invoke();
                    }
                );
            });
        }


        protected virtual void SetSnackbarTextView(Snackbar bar, ToastConfig cfg)
        {
            var group = (ViewGroup)bar.View;
            for (var i = 0; i < group.ChildCount; i++)
            {
                var txt = group.GetChildAt(i) as TextView;
                if (txt != null)
                {
                    var text = $"<b>{cfg.Title}</b>";
                    if (!String.IsNullOrWhiteSpace(cfg.Description))
                        text += $"\n<br /><i>{cfg.Description}</i>";

                    txt.SetTextColor(cfg.TextColor.ToNative());
                    txt.TextFormatted = Html.FromHtml(text);
                    return;
                }
            }
            throw new Exception("No textview found on snackbar");
        }

        #endregion

        #region Internals

        protected override IProgressDialog CreateDialogInstance()
        {
            return new ProgressDialog(this.TopActivityFunc());
        }


        protected virtual void Show(Activity activity, Dialog dialog)
        {
            activity.RunOnUiThread(dialog.Show);
        }


        protected virtual void Show(Activity activity, Android.App.AlertDialog.Builder builder)
        {
            var dialog = builder.Show();
            dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            activity.RunOnUiThread(dialog.Show);
        }


        protected virtual void ShowDialog<TFragment, TConfig>(FragmentActivity activity, TConfig config) where TFragment : AbstractDialogFragment<TConfig> where TConfig : class, new()
        {
            activity.RunOnUiThread(() =>
            {
                var frag = (TFragment)Activator.CreateInstance(typeof(TFragment));
                frag.Config = config;
                frag.Show(activity.FragmentManager, FragmentTag);
            });
        }


        protected virtual void ShowDialog<TFragment, TConfig>(AppCompatActivity activity, TConfig config) where TFragment : AbstractAppCompatDialogFragment<TConfig> where TConfig : class, new()
        {
            activity.RunOnUiThread(() =>
            {
                var frag = (TFragment)Activator.CreateInstance(typeof(TFragment));
                frag.Config = config;
                frag.Show(activity.SupportFragmentManager, FragmentTag);
            });
        }

        #endregion
    }
}