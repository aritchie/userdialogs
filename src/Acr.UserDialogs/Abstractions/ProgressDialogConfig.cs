using System;


namespace Acr.UserDialogs
{
    public class ProgressDialogConfig
    {
        public static string DefaultCancelText { get; set; } = "Cancel";
        public static string DefaultTitle { get; set; } = "Loading";
        public static MaskType DefaultMaskType { get; set; } = MaskType.Black;


        public string CancelText { get; set; }
        public string Title { get; set; }
        public bool AutoShow { get; set; }
        public bool IsDeterministic { get; set; }
        public MaskType MaskType { get; set; }
        public Action OnCancel { get; set; }


        public ProgressDialogConfig()
        {
            this.Title = DefaultTitle;
            this.CancelText = DefaultCancelText;
            this.MaskType = DefaultMaskType;
            this.AutoShow = true;
        }


        public ProgressDialogConfig SetCancel(string cancelText = null, Action onCancel = null)
        {
            if (cancelText != null)
                this.CancelText = cancelText;

            this.OnCancel = onCancel;
            return this;
        }


        public ProgressDialogConfig SetTitle(string title)
        {
            this.Title = title;
            return this;
        }


        public ProgressDialogConfig SetMaskType(MaskType maskType)
        {
            this.MaskType = maskType;
            return this;
        }


        public ProgressDialogConfig SetAutoShow(bool autoShow)
        {
            this.AutoShow = autoShow;
            return this;
        }


        public ProgressDialogConfig SetIsDeterministic(bool isDeterministic)
        {
            this.IsDeterministic = isDeterministic;
            return this;
        }
    }
}