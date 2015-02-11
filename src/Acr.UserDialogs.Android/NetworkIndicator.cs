using System;
using Android.App;
using Android.Views;


namespace Acr.UserDialogs {

    public class NetworkIndicator : INetworkIndicator {
        private readonly Activity activity;


        public NetworkIndicator(Activity activity) {
            this.activity = activity;
        }


		public bool IsShowing { get; private set; }


        public void Show() {
			Utils.RequestMainThread(() => {
				this.IsShowing = true;
				this.activity.SetProgressBarVisibility(true);
				this.activity.SetProgressBarIndeterminateVisibility(true);
			});
        }


        public void Hide() {
			Utils.RequestMainThread(() => {
				this.IsShowing = false;
				this.activity.SetProgressBarIndeterminateVisibility(false);
				this.activity.SetProgressBarVisibility(false);
			});
		}


        public void Dispose() {
            this.Hide();
        }
    }
}