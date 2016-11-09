using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;


// TODO: fix for immersive mode - http://stackoverflow.com/questions/22794049/how-to-maintain-the-immersive-mode-in-dialogs/23207365#23207365
//dialog.getWindow().setFlags(WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE, WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE);
namespace Acr.UserDialogs.Fragments
{
    public abstract class AbstractAppCompatDialogFragment<T> : AppCompatDialogFragment where T : class
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

            return this.CreateDialog(this.Config);
        }


        //public override void OnDetach()
        //{
        //    base.OnDetach();
        //    if (this.Dialog != null)
        //        this.Dialog.KeyPress -= this.OnKeyPress;
        //}


        protected abstract Dialog CreateDialog(T config);
        protected AppCompatActivity AppCompatActivity => this.Activity as AppCompatActivity;
    }


    public abstract class AbstractDialogFragment<T> : DialogFragment where T : class
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

            return this.CreateDialog(this.Config);
        }


        //public override void OnDetach()
        //{
        //    base.OnDetach();
        //    if (this.Dialog != null)
        //        this.Dialog.KeyPress -= this.OnKeyPress;
        //}


        protected abstract Dialog CreateDialog(T config);
    }
}