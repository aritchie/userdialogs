using System;

using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Acr.UserDialogs
{
    /// <summary>
    /// https://github.com/pedant/sweet-alert-dialog
    /// </summary>
    public class BaseInteractiveDialogFragment<TConfig> : AppCompatDialogFragment where TConfig : InteractiveAlertConfig
    {
        protected TConfig Config { get; set; }

        private ITopContentViewHolder topViewHolder;

        protected BaseInteractiveDialogFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        protected BaseInteractiveDialogFragment()
        {

        }

        public override void OnStart()
        {
            base.OnStart();
            this.topViewHolder?.OnStart();
        }

        public override void OnPause()
        {
            base.OnPause();
            this.topViewHolder?.OnPause();
        }

        public override Android.App.Dialog OnCreateDialog(Android.OS.Bundle savedInstanceState)
        {
            AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(this.Activity);
            alertDialogBuilder.SetCancelable(this.Config.IsCancellable);
            var doneConfig = this.Config.Done;
            if (doneConfig != null)
            {
                var title = doneConfig.Title;
                alertDialogBuilder.SetPositiveButton(title, (sender, e) =>
                {
                    var handler = doneConfig.Action;
                    if (handler != null)
                    {
                        handler();
                    }
                    else
                    {
                        this.Dismiss();
                    }
                });
            }

            if (this.Config.CustomButton != null)
            {
                var title = this.Config.CustomButton.Title;
                alertDialogBuilder.SetNegativeButton(title, (sender, e) =>
                {
                    var handler = this.Config.CustomButton.Action;
                    if (handler != null)
                    {
                        handler();
                    }
                    else
                    {
                        this.Dismiss();
                    }
                });
            }

            var contentView = (LinearLayout)LayoutInflater.From(this.Context).Inflate(Resource.Layout.alert_dialog, null);
            var bottomView = contentView.FindViewById<LinearLayout>(Resource.Id.alert_dialog_bottom);
            this.OnSetContentView(contentView);

            // try set bottom view
            bottomView.Visibility = this.OnSetBottomView(bottomView) ? ViewStates.Visible : ViewStates.Gone;

            var topContentView = contentView.FindViewById<FrameLayout>(Resource.Id.alert_dialog_top);
            this.topViewHolder = TopContentFactory.CreateTopViewHolder(this.Context, topContentView, this.Config.Style);
            this.topViewHolder.ContentView.RequestLayout();

            // set text
            this.SetContentText(contentView, Resource.Id.alert_dialog_title, this.Config.Title);
            this.SetContentText(contentView, Resource.Id.alert_dialog_content, this.Config.Message);
            alertDialogBuilder.SetView(contentView);

            return alertDialogBuilder.Create();
        }

        protected void SetContentText(View contentView, int textViewId, string text)
        {
            var textView = contentView.FindViewById<TextView>(textViewId);
            if (string.IsNullOrEmpty(text))
            {
                textView.Visibility = ViewStates.Gone;
            }
            else
            {
                textView.Text = text;
            }
        }

        protected virtual void OnSetContentView(ViewGroup viewGroup)
        {

        }

        protected virtual bool OnSetBottomView(ViewGroup viewGroup)
        {
            return false;
        }

        public static T NewInstance<T>(TConfig alertConfig) where T : BaseInteractiveDialogFragment<TConfig>
        {
            var dialogFragment = (T)Activator.CreateInstance(typeof(T));
            dialogFragment.Config = alertConfig;

            return dialogFragment;
        }
    }
}