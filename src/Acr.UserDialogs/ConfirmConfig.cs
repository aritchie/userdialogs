using System;


namespace Acr.UserDialogs
{

    public class ConfirmConfig : IStandardDialogConfig, IAndroidStyleDialogConfig, IUwpKeyboardEvents
    {
        public static string DefaultYes { get; set; } = "Yes";
        public static string DefaultNo { get; set; } = "No";
        public static string DefaultOkText { get; set; } = "Ok";
        public static string DefaultCancelText { get; set; } = "Cancel";
        public static int? DefaultAndroidStyleId { get; set; }


        public string Title { get; set; }
        public string Message { get; set; }
        public int? AndroidStyleId { get; set; } = DefaultAndroidStyleId;
        public Action<bool> OnAction { get; set; }
        public bool UwpCancelOnEscKey { get; set; }
        public bool UwpSubmitOnEnterKey { get; set; }

        public string OkText { get; set; } = DefaultOkText;
        public string CancelText { get; set; } = DefaultCancelText;


        public ConfirmConfig UseYesNo()
        {
            this.OkText = DefaultYes;
            this.CancelText = DefaultNo;
            return this;
        }


        public ConfirmConfig SetTitle(string title)
        {
            this.Title = title;
            return this;
        }


        public ConfirmConfig SetMessage(string message)
        {
            this.Message = message;
            return this;
        }


        public ConfirmConfig SetOkText(string text)
        {
            this.OkText = text;
            return this;
        }


        public ConfirmConfig SetAction(Action<bool> action)
        {
            this.OnAction = action;
            return this;
        }


        public ConfirmConfig SetCancelText(string text)
        {
            this.CancelText = text;
            return this;
        }
    }
}
