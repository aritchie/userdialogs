using System;


namespace Acr.UserDialogs {

    public class NetworkIndicator : IProgressDialog {

        public string Title { get; set; }
        public int PercentComplete { get; set; }
        public bool IsDeterministic { get; set; }


        public bool IsShowing {
            get { return true; }
        }


        public void SetCancel(Action onCancel, string cancelText = "Cancel") {}


        public void Show() {
        }


        public void Hide() {
        }


        public void Dispose() {
            this.Hide();
        }
    }
}