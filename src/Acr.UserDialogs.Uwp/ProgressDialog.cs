using System;


namespace Acr.UserDialogs {

    public class ProgressDialog : IProgressDialog {
        public bool IsDeterministic {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public bool IsShowing {
            get {
                throw new NotImplementedException();
            }
        }

        public MaskType MaskType {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public int PercentComplete {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public string Title {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public void Dispose() {
            throw new NotImplementedException();
        }

        public void Hide() {
            throw new NotImplementedException();
        }

        public void SetCancel(Action onCancel, string cancelText = "Cancel") {
            throw new NotImplementedException();
        }

        public void Show() {
            throw new NotImplementedException();
        }
    }
}
