using System;

namespace Acr.UserDialogs
{
    public class InteractiveAlertConfig : IStandardDialogConfig, IAndroidStyleDialogConfig
    {
        public static string DefaultDoneText { get; set; } = "Done";

        private InteractiveActionButton done;

        public static int? DefaultAndroidStyleId { get; set; }

        public InteractiveAlertStyle Style { get; set; } = InteractiveAlertStyle.Success;

        public InteractiveActionButton Done
        {
            get { return this.done; }
            set
            {
                this.done = value;
                if (string.IsNullOrEmpty(this.done.Title))
                {
                    this.done.Title = DefaultDoneText;
                }
            }
        }

        public InteractiveActionButton CustomButton { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public int? AndroidStyleId { get; set; } = DefaultAndroidStyleId;

        public bool IsCancellable { get; set; } = true;


        public class InteractiveActionButton
        {
            public string Title { get; set; }

            public Action Action { get; set; }
        }
    }
}