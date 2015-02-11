using System;
#if __UNIFIED__
using UIKit;
#else
using MonoTouch.UIKit;
#endif


namespace Acr.UserDialogs {

    public class NetworkIndicator : INetworkIndicator {

        public string Title { get; set; }


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