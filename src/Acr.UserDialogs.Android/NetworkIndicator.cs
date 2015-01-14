using System;
using Android.App;


namespace Acr.UserDialogs {

    public class NetworkIndicator : IProgressDialog {
        private readonly Activity activity;


        public NetworkIndicator(Activity activity) {
            this.activity = activity;
        }


        public string Title { get; set; }
        public int PercentComplete { get; set; }
        public bool IsDeterministic { get; set; }


        public bool IsShowing {
            get { return true; }
        }


        public void SetCancel(Action onCancel, string cancelText = "Cancel") {}


        public void Show() {
            Utils.RequestMainThread(() => {
                //this.activity.SetProgress(0);
                //this.activity.SetProgressBarVisibility(true);
                //this.activity.SetProgressBarIndeterminateVisibility(true);
                //this.activity.SetProgressBarIndeterminate(true);
            });
        }


        public void Hide() {
        }


        public void Dispose() {
            this.Hide();
        }
    }
}