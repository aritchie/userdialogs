using System;
using System.Windows;
using Microsoft.Phone.Shell;


namespace Acr.UserDialogs {

    public class NetworkIndicator : INetworkIndicator {
        private readonly ProgressIndicator progress = new ProgressIndicator();


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