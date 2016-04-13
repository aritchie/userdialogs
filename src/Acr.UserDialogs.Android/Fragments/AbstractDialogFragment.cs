using System;
using Android.App;
using Android.OS;
using Newtonsoft.Json;


namespace Acr.UserDialogs.Fragments
{
    public abstract class AbstractAppCompatDialogFragment<T> : Android.Support.V7.App.AppCompatDialogFragment where T : class
    {
        const string BundleKey = "UserDialogFragment";
        public T Config { get; set; }


        public override void OnSaveInstanceState(Bundle bundle)
        {
            base.OnSaveInstanceState(bundle);
            var configString = JsonConvert.SerializeObject(this.Config);
            bundle.PutString(BundleKey, configString);
        }


        public override Dialog OnCreateDialog(Bundle bundle)
        {
            if (this.Config == null)
            {
                var configString = bundle.GetString(BundleKey);
                this.Config = JsonConvert.DeserializeObject<T>(configString);
            }
            var dialog = this.CreateDialog(this.Config);
            //dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            //dialog.SetCancelable(false);
            //dialog.SetCanceledOnTouchOutside(false);
            return dialog;
        }


        protected abstract Dialog CreateDialog(T config);
    }


    public abstract class AbstractDialogFragment<T> : DialogFragment where T  : class
    {
        const string BundleKey = "UserDialogFragment";
        public T Config { get; set; }


        public override void OnSaveInstanceState(Bundle bundle)
        {
            base.OnSaveInstanceState(bundle);
            var configString = JsonConvert.SerializeObject(this.Config);
            bundle.PutString(BundleKey, configString);
        }


        public override Dialog OnCreateDialog(Bundle bundle)
        {
            if (this.Config == null)
            {
                var configString = bundle.GetString(BundleKey);
                this.Config = JsonConvert.DeserializeObject<T>(configString);
            }
            var dialog = this.CreateDialog(this.Config);
            //dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            //dialog.SetCancelable(false);
            //dialog.SetCanceledOnTouchOutside(false);
            return dialog;
        }


        protected abstract Dialog CreateDialog(T config);

    }
}