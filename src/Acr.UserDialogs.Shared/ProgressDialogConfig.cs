using System;


namespace Acr.UserDialogs {

    public class ProgressDialogConfig {

        public string CancelText { get; set; }
        public string Title { get; set; }
        public bool AutoShow { get; set; }
        public bool IsDeterministic { get; set; }
        public Action OnCancel { get; set; }


        public ProgressDialogConfig() {
            this.Title = "Loading";
            this.CancelText = "Cancel";
            this.AutoShow = true;
        }
    }
}

