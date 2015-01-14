using System;
using System.Windows;
using Microsoft.Phone.Shell;


namespace Acr.UserDialogs {

    public class NetworkIndicator : IProgressDialog {
        private readonly ProgressIndicator progress = new ProgressIndicator();

        public string Title {
            get { return this.progress.Text; }
            set { this.progress.Text = value; }
        }


        public int PercentComplete {
            get { return 0; }
            set {
                this.progress.Value = value;
            }
        }


        public bool IsDeterministic {
            get { return !this.progress.IsIndeterminate; }
            set { this.progress.IsIndeterminate = !value; }
        }


        public bool IsShowing {
            get { return this.progress.IsVisible; }
            private set {
                this.progress.IsVisible = value;
                SystemTray.SetProgressIndicator(Deployment.Current, value ? this.progress : null);
            }
        }


        public void SetCancel(Action onCancel, string cancelText = "Cancel") {}


        public void Show() {
            this.IsShowing = true;
        }


        public void Hide() {
            this.IsShowing = false;
        }


        public void Dispose() {
            this.Hide();
        }
    }
}