namespace Acr.UserDialogs
{
    using System;

    using Android.Runtime;

    public class InteractiveDialogFragment : BaseInteractiveDialogFragment<InteractiveAlertConfig>
    {
        public InteractiveDialogFragment()
        {

        }

        protected InteractiveDialogFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }
    }
}