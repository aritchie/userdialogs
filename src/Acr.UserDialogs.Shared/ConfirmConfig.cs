using System;


namespace Acr.UserDialogs {

    public class ConfirmConfig {

        public string Title { get; set; }
        public string Message { get; set; }
        public Action<bool> OnConfirm { get; set; }

        public string OkText { get; set; }
        public string CancelText { get; set; }


        public ConfirmConfig() {
            this.OkText = "OK";
            this.CancelText = "Cancel";
        }


        //public static ConfirmConfig Create(string message, Action<bool> onConfirm = null) {
        //    return new ConfirmConfig {
        //        Message = message,
        //        OnConfirm = onConfirm
        //    };
        //}


        public ConfirmConfig UseYesNo() {
            this.OkText = "Yes";
            this.CancelText = "No";
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
