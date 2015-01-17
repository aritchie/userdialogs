using System;
using UIKit;


namespace Acr.UserDialogs {

    public class NetworkIndicator : IProgressIndicator {

        public string Title { get; set; }
        public int PercentComplete { get; set; }
        public bool IsDeterministic { get; set; }


        public bool IsShowing {
            get { return UIApplication.SharedApplication.NetworkActivityIndicatorVisible; }
        }


        public void Show() {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
        }


        public void Hide() {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
        }


        public void Dispose() {
            this.Hide();
        }
    }
}