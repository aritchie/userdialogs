using System;


namespace Acr.UserDialogs {

    public class ProgressDialog : IProgressDialog {

        #region IProgressDialog Members

        private string text;
        public string Title {
            get { return this.text; }
            set {
                if (this.text == value)
                    return;

                this.text = value;
                //this.Refresh();
            }
        }


        private int percentComplete;
        public int PercentComplete {
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

                //this.Refresh();
            }
        }


        public bool IsDeterministic { get; set; }


        public bool IsShowing { get; private set; }


        public void SetCancel(Action onCancel, string cancelText) {
        }


        public void Show() {
            if (this.IsShowing)
                return;

            this.IsShowing = true;
        }


        public void Hide() {
            if (!this.IsShowing)
                return;

            this.IsShowing = false;
        }


        #endregion

        #region IDisposable Members

        public void Dispose() {
            this.Hide();
        }

        #endregion
    }
}
