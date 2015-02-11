using System;


namespace Acr.UserDialogs {

    public class NetworkIndicator : INetworkIndicator {

        public bool IsShowing {
            get { return true; }
        }


        public void Show() {
        }


        public void Hide() {
        }


        public void Dispose() {
            this.Hide();
        }
    }
}