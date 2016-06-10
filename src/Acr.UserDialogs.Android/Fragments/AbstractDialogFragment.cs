using System;
using Android.App;
using Android.OS;
using Android.Views;


namespace Acr.UserDialogs.Fragments
{
    public abstract class AbstractAppCompatDialogFragment<T> : Android.Support.V7.App.AppCompatDialogFragment where T : class
    {
        public T Config { get; set; }


        public override void OnSaveInstanceState(Bundle bundle)
        {
            base.OnSaveInstanceState(bundle);
            ConfigStore.Instance.Store(bundle, this.Config);
        }


        public override Dialog OnCreateDialog(Bundle bundle)
        {
            if (this.Config == null)
                this.Config = ConfigStore.Instance.Pop<T>(bundle);

            var dialog = this.CreateDialog(this.Config);
            //dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            dialog.SetCancelable(false);
            dialog.SetCanceledOnTouchOutside(false);

            return dialog;
        }


        protected abstract Dialog CreateDialog(T config);
    }


    public abstract class AbstractDialogFragment<T> : DialogFragment where T  : class
    {
        public T Config { get; set; }


        public override void OnSaveInstanceState(Bundle bundle)
        {
            base.OnSaveInstanceState(bundle);
            ConfigStore.Instance.Store(bundle, this.Config);
        }


        public override Dialog OnCreateDialog(Bundle bundle)
        {
            if (this.Config == null)
                this.Config = ConfigStore.Instance.Pop<T>(bundle);

            var dialog = this.CreateDialog(this.Config);
            dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            dialog.SetCancelable(false);
            dialog.SetCanceledOnTouchOutside(false);
            return dialog;
        }


        protected abstract Dialog CreateDialog(T config);

    }
}