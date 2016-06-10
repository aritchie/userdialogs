using System;


namespace Acr.UserDialogs
{

    public class AlertConfig
    {
        public static string DefaultOkText { get; set; } = "Ok";
        public static int? DefaultAndroidStyleId { get; set; }

        public string OkText { get; set; } = DefaultOkText;
        public string Title { get; set; }
        public string Message { get; set; }
        public int? AndroidStyleId { get; set; } = DefaultAndroidStyleId;
        public Action OnOk { get; set; }


        public AlertConfig SetOkText(string text)
        {
            this.OkText = text;
            return this;
        }


        public AlertConfig SetTitle(string title)
        {
            this.Title = title;
            return this;
        }


        public AlertConfig SetMessage(string message)
        {
            this.Message = message;
            return this;
        }


        public AlertConfig SetCallback(Action onOk)
        {
            this.OnOk = onOk;
            return this;
        }
    }
}
