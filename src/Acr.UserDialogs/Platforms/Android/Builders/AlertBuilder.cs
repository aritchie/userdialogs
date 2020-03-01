using System;
using Android.App;
using AlertDialog = Android.App.AlertDialog;
#if ANDROIDX
using AndroidX.AppCompat.App;
using AppCompatAlertDialog = AndroidX.AppCompat.App.AlertDialog;
#else
using Android.Support.V7.App;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;
#endif


namespace Acr.UserDialogs.Builders
{
    public class AlertBuilder : IAlertDialogBuilder<AlertConfig>
    {
        public Dialog Build(Activity activity, AlertConfig config)
        {
            //var layout = new LinearLayout(context) {
            //    Orientation = Orientation.Vertical,
            //    OverScrollMode = OverScrollMode.IfContentScrolls
            //};
            //var txt = new TextView(context);

            return new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (o, e) => config.OnAction?.Invoke())
                .Create();
        }


        public Dialog Build(AppCompatActivity activity, AlertConfig config)
        {
            return new AppCompatAlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (o, e) => config.OnAction?.Invoke())
                .Create();
        }
    }
}