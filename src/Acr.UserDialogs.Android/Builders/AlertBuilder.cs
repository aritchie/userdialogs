using System;
using Android.App;
#if APPCOMPAT
using AlertDialog = Android.Support.V7.App.AlertDialog;
#else
using AlertDialog = Android.App.AlertDialog;
#endif


namespace Acr.UserDialogs.Builders
{
    public static class AlertBuilder
    {
        public static AlertDialog.Builder Build(Activity activity, AlertConfig config)
        {
            //var layout = new LinearLayout(context) {
            //    Orientation = Orientation.Vertical,
            //    OverScrollMode = OverScrollMode.IfContentScrolls
            //};
            //var txt = new TextView(context);

            return new AlertDialog
                .Builder(activity)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (o, e) => config.OnOk?.Invoke());
        }
    }
}