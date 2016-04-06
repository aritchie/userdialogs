//using System;
//using System.Collections.Generic;
//using Acr.Support.Android;
//using Acr.UserDialogs;
//using Android.App;
//using Android.OS;
//using Android.Support.V7.App;
//using AlertDialog = Android.Support.V7.App.AlertDialog;
//using Utils = Acr.Support.Android.Extensions;

//namespace Acr.UserDialogs.Fragments
//{
//    public class FragmentUserDialogs : UserDialogsImpl
//    {
//        private const string ConfigExtra = "EXTRA_DIALOG_CONFIGURATION";

//        private long _configCounter = 0;
//        private readonly IDictionary<long, object> _configStore = new Dictionary<long, object>();

//        private long StoreConfig(object config)
//        {
//            var cnt = this._configCounter++;
//            this._configStore[cnt] = config;
//            return cnt;
//        }

//        private T LoadConfig<T>(long id)
//        {
//            var val = this._configStore[id];
//            this._configStore.Remove(id);
//            return (T)val;
//        }

//        private abstract class UserDialogFragment<TConfig> : AppCompatDialogFragment
//        {
//            public new const string Tag = "UserDialogFragment";

//            protected FragmentUserDialogs Master => UserDialogs.Instance as FragmentUserDialogs;

//            private TConfig _alertConfig;

//            public sealed override bool Cancelable
//            {
//                get { return base.Cancelable; }
//                set { base.Cancelable = value; }
//            }

//            protected UserDialogFragment()
//            {
//                this.Cancelable = false;
//            }

//            protected UserDialogFragment(TConfig alertConfig) : this()
//            {
//                _alertConfig = alertConfig;
//            }

//            public sealed override void OnSaveInstanceState(Bundle outState)
//            {
//                base.OnSaveInstanceState(outState);
//                outState.PutLong(FragmentUserDialogs.ConfigExtra, this.Master.StoreConfig(this._alertConfig));
//            }

//            public sealed override Dialog OnCreateDialog(Bundle savedInstanceState)
//            {
//                if(this._alertConfig == null)
//                    this._alertConfig = this.Master.LoadConfig<TConfig>(savedInstanceState.GetLong(FragmentUserDialogs.ConfigExtra));

//                var dialog = this.CreateDialog(this._alertConfig);
//                dialog.SetCancelable(false);
//                dialog.SetCanceledOnTouchOutside(false);
//                return dialog;
//            }

//            protected abstract Dialog CreateDialog(TConfig config);
//        }

//        private class AlertDialogFragment : UserDialogFragment<AlertConfig>
//        {
//            public new const string Tag = "AlertDialogFragment";

//            public AlertDialogFragment() { }

//            public AlertDialogFragment(AlertConfig config) : base(config) { }

//            protected override Dialog CreateDialog(AlertConfig config)
//            {
//                return new AlertDialog
//                    .Builder(this.Master.GetTopActivity())
//                    .SetCancelable(false)
//                    .SetMessage(config.Message)
//                    .SetTitle(config.Title)
//                    .SetPositiveButton(config.OkText, (o, e) => config.OnOk?.Invoke())
//                    .Create();
//            }
//        }

//        private class ConfirmDialogFragment : UserDialogFragment<ConfirmConfig>
//        {
//            public new const string Tag = "ConfirmDialogFragment";

//            public ConfirmDialogFragment() { }

//            public ConfirmDialogFragment(ConfirmConfig config) : base(config) { }

//            protected override Dialog CreateDialog(ConfirmConfig config)
//            {
//                return new AlertDialog
//                    .Builder(this.Master.GetTopActivity())
//                    .SetCancelable(false)
//                    .SetMessage(config.Message)
//                    .SetTitle(config.Title)
//                    .SetPositiveButton(config.OkText, (s, a) => config.OnConfirm(true))
//                    .SetNegativeButton(config.CancelText, (s, a) => config.OnConfirm(false))
//                    .Create();
//            }
//        }

//        public static void Init()
//        {
//            ActivityLifecycleCallbacks.Register((Application)Application.Context.ApplicationContext);
//            UserDialogs.Instance = new FragmentUserDialogs(() => ActivityLifecycleCallbacks.CurrentTopActivity);
//        }

//        private FragmentUserDialogs(Func<Activity> getTopActivity) : base(getTopActivity) {}

//        private new AppCompatActivity GetTopActivity()
//        {
//            return (AppCompatActivity)base.GetTopActivity();
//        }

//        private void ShowDialog<TFragment, TConfig>(TConfig config) where TFragment : UserDialogFragment<TConfig>, new()
//        {
//            Utils.RequestMainThread(() =>
//            {
//                var frag = (TFragment) Activator.CreateInstance(typeof(TFragment), config);
//                frag.Show(this.GetTopActivity().SupportFragmentManager, AlertDialogFragment.Tag);
//            });
//        }
        
//        public override void Alert(AlertConfig config)
//        {
//            this.ShowDialog<AlertDialogFragment, AlertConfig>(config);
//        }

//        public override void Confirm(ConfirmConfig config)
//        {
//            this.ShowDialog<ConfirmDialogFragment, ConfirmConfig>(config);
//        }
//    }
//}