using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;


namespace Acr.UserDialogs
{
    public class DateTimeDialog : Dialog, IDialogInterfaceOnDismissListener, IDialogInterfaceOnCancelListener
    {
        public DateTimeDialog(Context context, bool cancelable, EventHandler cancelHandler) : base(context, cancelable, cancelHandler) {}
        public DateTimeDialog(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) {}
        public DateTimeDialog(Context context) : base(context) {}
        public DateTimeDialog(Context context, bool cancelable, IDialogInterfaceOnCancelListener cancelListener) : base(context, cancelable, cancelListener) {}
        public DateTimeDialog(Context context, int themeResId) : base(context, themeResId) {}

        public void Init(DateTimePromptConfig config)
        {

        }


        public static void BuildOnLayout(LinearLayout layout, DateTimePromptConfig config)
        {
            var ctx = layout.Context;

        }

        public void OnDismiss(IDialogInterface dialog)
        {
        }


        public void OnCancel(IDialogInterface dialog)
        {
        }
    }


    public class AppCompatDateTimeDialog : AppCompatDialog
    {
        public AppCompatDateTimeDialog(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) {}
        public AppCompatDateTimeDialog(Context context, bool cancelable, IDialogInterfaceOnCancelListener cancelListener) : base(context, cancelable, cancelListener) {}
        public AppCompatDateTimeDialog(Context context, int theme) : base(context, theme) {}
        public AppCompatDateTimeDialog(Context context) : base(context) {}


        public void Init(DateTimePromptConfig config)
        {
            var layout = new LinearLayout(this.Context);
            DateTimeDialog.BuildOnLayout(layout, config);
            //this.SetContentView(layout, ViewGroup.LayoutParams.MatchParent);
        }
    }
}