using System;
using Android.App;
using AndroidHUD;


namespace Acr.UserDialogs
{
    public class ProgressDialog : IProgressDialog
    {
        readonly Activity activity;
        readonly ProgressDialogConfig config;


        public ProgressDialog(ProgressDialogConfig config, Activity activity)
        {
            this.config = config;
            this.activity = activity;
        }

        #region IProgressDialog Members

        string title;
        public virtual string Title
        {
            get { return this.title; }
            set
            {
                if (this.title == value)
                    return;

                this.title = value;
                this.Refresh();
            }
        }


        int percentComplete;
        public virtual int PercentComplete
        {
            get { return this.percentComplete; }
            set
            {
                if (this.percentComplete == value)
                    return;

                if (value > 100)
                    this.percentComplete = 100;
                else if (value < 0)
                    this.percentComplete = 0;
                else
                    this.percentComplete = value;

                this.Refresh();
            }
        }


        public virtual bool IsShowing { get; private set; }


        public virtual void Show()
        {
            if (this.IsShowing)
                return;

            this.IsShowing = true;
            this.Refresh();
        }


        public virtual void Hide()
        {
            this.IsShowing = false;
            try
            {
                AndHUD.Shared.Dismiss();
            }
            catch { }
        }

        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            this.Hide();
        }

        #endregion

        #region Internals

        protected virtual void Refresh()
        {
            if (!this.IsShowing)
                return;

            var p = -1;
            var txt = this.Title;
            if (this.config.IsDeterministic)
            {
                p = this.PercentComplete;
                if (!String.IsNullOrWhiteSpace(txt))
                    txt += "\n";

                txt += p + "%\n";
            }

            if (this.config.OnCancel != null)
                txt += "\n" + this.config.CancelText;

            // I'll help out andhud here
            this.activity.SafeRunOnUi(() =>
                AndHUD.Shared.Show(
                    this.activity,
                    txt,
                    p,
                    this.config.MaskType.ToNative(),
                    null,
                    this.OnCancelClick
                )
            );
        }


        void OnCancelClick()
        {
            if (this.config.OnCancel == null)
                return;

            this.Hide();
            this.config.OnCancel();
        }

        #endregion
    }
}