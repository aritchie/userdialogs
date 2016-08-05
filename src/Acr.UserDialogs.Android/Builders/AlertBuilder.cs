using System;
using Android.App;
using Android.Support.V7.App;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;


namespace Acr.UserDialogs.Builders
{
    public class AlertBuilder : AbstractAlertDialogBuilder<AlertConfig>
    {
        public override AlertDialog.Builder Build(Activity activity, AlertConfig config)
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
                .SetPositiveButton(config.OkText, (o, e) => config.OnAction?.Invoke());
        }


        public override AppCompatAlertDialog.Builder Build(AppCompatActivity activity, AlertConfig config)
        {
            return this
                .CreateBaseBuilder(activity, config.AndroidStyleId)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (o, e) => config.OnAction?.Invoke());
        }
    }
}