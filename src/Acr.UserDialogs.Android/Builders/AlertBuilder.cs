using System;
using Android.App;
using Android.Support.V7.App;
using Android.Views;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;


namespace Acr.UserDialogs.Builders
{
    public class AlertBuilder
    {
        public Dialog Build(Activity activity, AlertConfig config)
        {
            //var layout = new LinearLayout(context) {
            //    Orientation = Orientation.Vertical,
            //    OverScrollMode = OverScrollMode.IfContentScrolls
            //};
            //var txt = new TextView(context);

            var builder = new AlertDialog
                .Builder(activity)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.Positive.Text, (o, e) => config.OnAction?.Invoke(DialogChoice.Positive));

            if (config.Negative.IsVisible)
            {
                builder.SetNegativeButton(config.Negative.Text, (o, e) => config.OnAction?.Invoke(DialogChoice.Negative));
            }

            if (config.Neutral.IsVisible)
            {
                builder.SetNeutralButton(config.Neutral.Text, (o, e) => config.OnAction?.Invoke(DialogChoice.Neutral));
            }

            return this.SetDialogIntrisics(builder.Create(), config);
        }


        public Dialog Build(AppCompatActivity activity, AlertConfig config)
        {
            var builder = new AppCompatAlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.Positive.Text, (o, e) => config.OnAction?.Invoke(DialogChoice.Positive));

            if (config.Negative.IsVisible)
            {
                builder.SetNegativeButton(config.Negative.Text, (o, e) => config.OnAction?.Invoke(DialogChoice.Negative));
            }

            if (config.Neutral.IsVisible)
            {
                builder.SetNeutralButton(config.Neutral.Text, (o, e) => config.OnAction?.Invoke(DialogChoice.Neutral));
            }

            return this.SetDialogIntrisics(builder.Create(), config);
        }


        protected virtual Dialog SetDialogIntrisics(Dialog dialog, AlertConfig config) 
        {            
            dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            dialog.SetCancelable(false);
            dialog.SetCanceledOnTouchOutside(false);
            //dialog.KeyPress += this.OnKeyPress;

            return dialog;
        }

        //protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        //{
        //    base.OnKeyPress(sender, args);
        //    if (args.KeyCode != Keycode.Back)
        //        return;

        //    args.Handled = true;
        //    this.Config?.OnAction?.Invoke();
        //    this.Dismiss();
        //}
    }
}