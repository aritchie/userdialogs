using System;
using BigTed;


namespace Acr.UserDialogs {

    public class ProgressDialog : IProgressDialog {

        #region IProgressDialog Members

        private string title;
        public virtual string Title {
            get { return this.title; }
            set {
                if (this.title == value)
                    return;

                this.title = value;
                this.Refresh();
            }
        }


        private int percentComplete;
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


        private string cancelText;
        private Action cancelAction;
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
            BTProgressHUD.Dismiss();
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

            if (this.cancelAction == null) {
                BTProgressHUD.Show(
                    this.Title,
                    p,
                    ProgressHUD.MaskType.Black
                );
            }
            else {
                BTProgressHUD.Show(
                    this.cancelText, 
                    this.cancelAction,
                    txt,
                    p,
                    ProgressHUD.MaskType.Black
                );
            }
        }

        #endregion
    }
}