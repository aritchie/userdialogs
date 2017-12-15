//using System;
//using Android.Content;
//using Android.OS;
//using Android.Support.V7.App;
//using AndroidHUD;


//namespace Acr.UserDialogs.Fragments
//{
//    public class LoadingFragment : AppCompatDialogFragment
//    {
//        public ProgressDialog Config { get; set; }


//        public override void OnSaveInstanceState(Bundle bundle)
//        {
//            base.OnSaveInstanceState(bundle);
//            ConfigStore.Instance.Store(bundle, this.Config);
//        }


//        public override void OnViewStateRestored(Bundle bundle)
//        {
//            base.OnViewStateRestored(bundle);
//            if (this.Config == null)
//                this.Config = ConfigStore.Instance.Pop<ProgressDialog>(bundle);
//        }


//        public override void OnAttach(Context context)
//        {
//            base.OnAttach(context);
//            if (this.Config.IsShowing)
//                this.Config.Show();
//        }


//        public override void OnDetach()
//        {
//            base.OnDetach();
//            try
//            {
//                AndHUD.Shared.Dismiss(this.Activity);
//            }
//            catch { }
//        }
//    }
//}