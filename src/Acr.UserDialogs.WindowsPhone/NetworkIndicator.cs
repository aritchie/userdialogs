using System;
using System.Windows;
using Microsoft.Phone.Shell;


namespace Acr.UserDialogs {

    public class NetworkIndicator : IProgressIndicator {
        private readonly ProgressIndicator progress = new ProgressIndicator();


        private int percentComplete;
        public int PercentComplete {
            get { return this.percentComplete; }
            set {
                if (this.percentComplete == value)
                    return;

                this.progress.IsIndeterminate = false;
                if (value > 100)
                    this.percentComplete = 100;

                else if (value < 0)
                    this.percentComplete = 0;

                else
                    this.percentComplete = value;

                this.progress.Value = this.percentComplete;
            }
        }


        public bool IsShowing {
            get { return this.progress.IsVisible; }
            private set {
                this.progress.IsVisible = value;
                SystemTray.SetProgressIndicator(Deployment.Current, value ? this.progress : null);
            }
        }


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