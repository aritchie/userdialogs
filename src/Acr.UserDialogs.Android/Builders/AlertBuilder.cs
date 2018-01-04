using System;
using Acr.UserDialogs.Utils;
using Android.App;
using Android.Support.V7.App;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;


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
                .SetTitle(!config.IsHtmlFormat ? config.Title : null)
                .SetMessage(!config.IsHtmlFormat ? config.Message : null)
                .SetView(config.IsHtmlFormat ? AlertDialogUtils.GetContentView(activity, config.Title, config.Message) : null)
                .SetPositiveButton(config.OkText, (o, e) => config.OnAction?.Invoke())
                .Create();
        }


        public Dialog Build(AppCompatActivity activity, AlertConfig config)
        {
            return new AppCompatAlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetTitle(!config.IsHtmlFormat ? config.Title : null)
                .SetMessage(!config.IsHtmlFormat ? config.Message : null)
                .SetView(config.IsHtmlFormat ? AlertDialogUtils.GetContentView(activity, config.Title, config.Message) : null)
                .SetPositiveButton(config.OkText, (o, e) => config.OnAction?.Invoke())
                .Create();
        }
    }
}