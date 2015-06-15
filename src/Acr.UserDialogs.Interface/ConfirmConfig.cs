using System;


namespace Acr.UserDialogs {

    public class ConfirmConfig {
        public static string DefaultYes { get; set; }
        public static string DefaultNo { get; set; }
        public static string DefaultOkText { get; set; }
        public static string DefaultCancelText { get; set; }

        static ConfirmConfig() {
            DefaultYes = "Yes";
            DefaultNo = "No";
            DefaultOkText = "Ok";
            DefaultCancelText = "Cancel";
        }


        public string Title { get; set; }
        public string Message { get; set; }
        public Action<bool> OnConfirm { get; set; }

        public string OkText { get; set; }
        public string CancelText { get; set; }


        public ConfirmConfig() {
            this.OkText = DefaultOkText;
            this.CancelText = DefaultCancelText;
        }


        public ConfirmConfig UseYesNo() {
            this.OkText = DefaultYes;
            this.CancelText = DefaultNo;
            return this;
        }


        public ConfirmConfig SetTitle(string title) {
            this.Title = title;
            return this;
        }


        public ConfirmConfig SetMessage(string message) {
            this.Message = message;
            return this;
        }


        public ConfirmConfig SetOkText(string text) {
            this.OkText = text;
            return this;
        }


        public ConfirmConfig SetCancelText(string text) {
            this.CancelText = text;
            return this;
        }
    }
}
