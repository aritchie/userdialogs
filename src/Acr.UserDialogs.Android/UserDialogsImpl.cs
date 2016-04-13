using System;
using Acr.UserDialogs.Fragments;
using Android.App;
using Android.Views;
using AndroidHUD;
using Splat;
using Utils = Acr.Support.Android.Extensions;

#if APPCOMPAT
using Acr.UserDialogs.Fragments;
using Android.Support.V7.App;
using Android.Graphics.Drawables;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Widget;
#else
using Acr.UserDialogs.Builders;
#endif

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


        public override void Alert(AlertConfig config)
        {
            this.ShowDialog<AlertDialogFragment, AlertConfig>(config);
        }


        public override void ActionSheet(ActionSheetConfig config)
        {
            this.ShowDialog<ActionSheetDialogFragment, ActionSheetConfig>(config);
        }


        public override void Confirm(ConfirmConfig config)
        {
            this.ShowDialog<ConfirmDialogFragment, ConfirmConfig>(config);
        }


        public override void DateTimePrompt(DateTimePromptConfig config)
        {
            this.ShowDialog<DateTimeDialogFragment, DateTimePromptConfig>(config);
        }


        public override void Login(LoginConfig config)
        {
            this.ShowDialog<LoginDialogFragment, LoginConfig>(config);
        }


        public override void Prompt(PromptConfig config)
        {
            this.ShowDialog<PromptDialogFragment, PromptConfig>(config);
        }


        public override void ShowImage(IBitmap image, string message, int timeoutMillis)
        {
            Utils.RequestMainThread(() =>
                AndHUD.Shared.ShowImage(this.TopActivityFunc(), image.ToNative(), message, AndroidHUD.MaskType.Black, TimeSpan.FromMilliseconds(timeoutMillis))
            );
        }


        public override void ShowSuccess(string message, int timeoutMillis)
        {
            Utils.RequestMainThread(() =>
                AndHUD.Shared.ShowSuccess(this.TopActivityFunc(), message, timeout: TimeSpan.FromMilliseconds(timeoutMillis))
            );
        }


        public override void ShowError(string message, int timeoutMillis)
        {
            Utils.RequestMainThread(() =>
                AndHUD.Shared.ShowError(this.TopActivityFunc(), message, timeout: TimeSpan.FromMilliseconds(timeoutMillis))
            );
        }


        protected override IProgressDialog CreateDialogInstance()
        {
            return new ProgressDialog(this.TopActivityFunc());
        }


        protected virtual void ShowDialog<TFragment, TConfig>(TConfig config) where TFragment : AbstractDialogFragment<TConfig> where TConfig : class, new()
        {
            Utils.RequestMainThread(() =>
            {
                var activity = this.TopActivityFunc();
                var frag = (TFragment)Activator.CreateInstance(typeof(TFragment));
                frag.Config = config;
                frag.Show(activity.SupportFragmentManager, FragmentTag);
            });
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
        public override void Toast(ToastConfig cfg)
        {
            Utils.RequestMainThread(() =>
            {
                var top = this.TopActivityFunc();
                var txt = cfg.Title;
                if (!String.IsNullOrWhiteSpace(cfg.Description))
                    txt += Environment.NewLine + cfg.Description;

                AndHUD.Shared.ShowToast(
                    top,
                    txt,
                    AndroidHUD.MaskType.Black,
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
#endif
    }
}