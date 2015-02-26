using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Acr.UserDialogs {

    public class ProgressDialog : IProgressDialog {
        private readonly ProgressBar progress = new ProgressBar();


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

                this.progress.Value = this.percentComplete;
            }
        }


        public bool IsDeterministic {
            get { return !this.progress.IsIndeterminate; }
            set { this.progress.IsIndeterminate = !value; }
        }


        public bool IsShowing {
            get { return (this.progress.Visibility == Visibility.Visible); }
            private set {

                this.progress.Visibility = value
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }


        public void SetCancel(Action onCancel, string cancelText) {
            this.progress.IsTapEnabled = true;
            this.progress.Tapped += (sender, args) => onCancel();
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
