using System;


namespace Acr.UserDialogs {

    public class ProgressConfig {

        public bool AutoShow { get; set; }
        public string Title { get; set; }
        public bool IsDeterministic { get; set; }

        public string CancelText { get; set; }
        public Action OnCancel { get; set; }


        public ProgressConfig() {
            this.AutoShow = true;
            this.Title = "Loading";
            this.CancelText = "Cancel";
        }
    }
}
