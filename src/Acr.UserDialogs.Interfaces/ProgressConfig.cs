using System;


namespace Acr.UserDialogs {

    public class ProgressConfig {
        public bool AutoShow { get; set; }
        public string Title { get; set; }
        public bool IsDeterministic { get; set; }

        public ProgressConfig() {
            this.AutoShow = true;
        }
    }
}
