using System;
using BigTed;
using UIKit;


namespace Acr.UserDialogs {

    public class ProgressDialog : IProgressDialog {

        public ProgressDialog() {
            this.MaskType = MaskType.Black;
        }


        #region IProgressDialog Members

        string title;
        public virtual string Title {
            get { return this.title; }
            set {
                if (this.title == value)
                    return;

                this.title = value;
                this.Refresh();
            }
        }


        public MaskType MaskType { get; set; }


        int percentComplete;
        public virtual int PercentComplete {
            get { return this.percentComplete; }
            set {
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


        public virtual bool IsDeterministic { get; set; }
        public virtual bool IsShowing { get; private set; }


        string cancelText;
        Action cancelAction;
        public virtual void SetCancel(Action onCancel, string cancel) {
            this.cancelAction = onCancel;
            this.cancelText = cancel;
            this.Refresh();
        }


        public virtual void Show() {
            this.IsShowing = true;
            this.Refresh();
        }


        public virtual void Hide() {
            this.IsShowing = false;
            UIApplication.SharedApplication.InvokeOnMainThread(BTProgressHUD.Dismiss);
        }

        #endregion

        #region IDisposable Members

        public virtual void Dispose() {
            this.Hide();
        }

        #endregion

        #region Internals

        protected virtual void Refresh() {
            if (!this.IsShowing)
                return;

            var txt = this.Title;
            float p = -1;
            if (this.IsDeterministic) {
                p = (float)this.PercentComplete / 100;
                if (!String.IsNullOrWhiteSpace(txt)) {
                    txt += "... ";
                }
                txt += this.PercentComplete + "%";
            }

            UIApplication.SharedApplication.InvokeOnMainThread(() => {
                if (this.cancelAction == null) {
                    BTProgressHUD.Show(
                        this.Title,
                        p,
                        this.MaskType.ToNative()
                    );
                }
                else {
                    BTProgressHUD.Show(
                        this.cancelText,
                        this.cancelAction,
                        txt,
                        p,
                        this.MaskType.ToNative()
                    );
                }
            });
        }

        #endregion
    }
}