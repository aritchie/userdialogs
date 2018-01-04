using System;


namespace Acr.UserDialogs
{

    public class AlertConfig : IStandardDialogConfig, IAndroidStyleDialogConfig
    {
        public static string DefaultOkText { get; set; } = "Ok";
        public static int? DefaultAndroidStyleId { get; set; }
        public static bool DefaultIsHtmlFormat { get; set; } = false;

        public string OkText { get; set; } = DefaultOkText;
        public string Title { get; set; }
        public string Message { get; set; }
        public int? AndroidStyleId { get; set; } = DefaultAndroidStyleId;
        public Action OnAction { get; set; }
        public bool IsHtmlFormat { get; set; } = DefaultIsHtmlFormat;


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


        public AlertConfig SetAction(Action action)
        {
            this.OnAction = action;
            return this;
        }

        public AlertConfig SetIsHtmlFormat(bool isHtmlFormat)
        {
            this.IsHtmlFormat = isHtmlFormat;
            return this;
        }
    }
}
