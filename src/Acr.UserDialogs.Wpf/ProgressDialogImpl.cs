using System;
#if WPF
using Ookii.Dialogs.Wpf;
#else
using Ookii.Dialogs;
#endif


namespace Acr.UserDialogs
{
    public class ProgressDialogImpl : IProgressDialog
    {
        readonly ProgressDialog dialog = new ProgressDialog();


        public void Dispose()
        {
            this.dialog.Dispose();
        }

        public string Title { get; set; }
        public int PercentComplete { get; set; }
        public bool IsDeterministic { get; set; }
        public bool IsShowing { get; }
        public MaskType MaskType { get; set; }


        public void Show()
        {
            this.dialog.Show();
        }

        public void Hide()
        {
        }

        public void SetCancel(Action onCancel, string cancelText = "Cancel")
        {
            throw new NotImplementedException();
        }
    }
}
