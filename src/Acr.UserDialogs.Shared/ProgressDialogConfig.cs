using System;


namespace Acr.UserDialogs {

    public class ProgressDialogConfig {

        public string CancelText { get; set; }
        public string Title { get; set; }
        public bool AutoShow { get; set; }
        public bool IsDeterministic { get; set; }
        public MaskType MaskType { get; set; }
        public Action OnCancel { get; set; }


        public ProgressDialogConfig() {
            this.Title = "Loading";
            this.CancelText = "Cancel";
            this.MaskType = MaskType.Black;
            this.AutoShow = true;
        }


        public ProgressDialogConfig SetCancel(string cancelText = null, Action onCancel = null) {
            if (cancelText != null)
                this.CancelText = cancelText;

            this.OnCancel = onCancel;
            return this;
        }


        public ProgressDialogConfig SetTitle(string title) {
            this.Title = title;
            return this;
        }


        public ProgressDialogConfig SetMaskType(MaskType maskType) {
            this.MaskType = maskType;
            return this;
        }


        public ProgressDialogConfig SetAutoShow(bool autoShow) {
            this.AutoShow = autoShow;
            return this;
        }


        public ProgressDialogConfig SetIsDeterministic(bool isDeterministic) {
            this.IsDeterministic = isDeterministic;
            return this;
        }
    }
}