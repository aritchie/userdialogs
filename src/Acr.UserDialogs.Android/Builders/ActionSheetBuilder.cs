using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Support.V7.App;
using Android.Views;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;


namespace Acr.UserDialogs.Builders
{
    public class ActionSheetBuilder
    {
        public Dialog Build(Activity activity, ActionSheetConfig config)
        {
            var dlg = new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetTitle(config.Title);
            //.SetCustomTitle(new TextView(activity) {
            //    Text = config.Title,
            //    TextSize = 18.0f
            //});

            if (config.ItemIcon != null || config.Options.Any(x => x.ItemIcon != null))
            {
                var adapter = new ActionSheetListAdapter(activity, Android.Resource.Layout.SelectDialogItem,
                    Android.Resource.Id.Text1, config);
                dlg.SetAdapter(adapter, (s, a) => config.Options[a.Which].Action?.Invoke());
            }
            else
            {
                var array = config
                    .Options
                    .Select(x => x.Text)
                    .ToArray();

                dlg.SetItems(array, (s, args) => config.Options[args.Which].Action?.Invoke());
            }

            if (config.Destructive != null)
                dlg.SetNegativeButton(config.Destructive.Text, (s, a) => config.Destructive.Action?.Invoke());

            if (config.Cancel != null)
                dlg.SetNeutralButton(config.Cancel.Text, (s, a) => config.Cancel.Action?.Invoke());

            return this.SetDialogDefaults(dlg.Create(), config);
        }


        public Dialog Build(AppCompatActivity activity, ActionSheetConfig config)
        {
            var dlg = new AppCompatAlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetTitle(config.Title);
            //.SetCustomTitle(new TextView(activity) {
            //    Text = config.Title,
            //    TextSize = 18.0f
            //});

            if (config.ItemIcon != null || config.Options.Any(x => x.ItemIcon != null))
            {
                var adapter = new ActionSheetListAdapter(activity, Android.Resource.Layout.SelectDialogItem,
                    Android.Resource.Id.Text1, config);
                dlg.SetAdapter(adapter, (s, a) => config.Options[a.Which].Action?.Invoke());
            }
            else
            {
                var array = config
                    .Options
                    .Select(x => x.Text)
                    .ToArray();

                dlg.SetItems(array, (s, args) => config.Options[args.Which].Action?.Invoke());
            }

            if (config.Destructive != null)
                dlg.SetNegativeButton(config.Destructive.Text, (s, a) => config.Destructive.Action?.Invoke());

            if (config.Cancel != null)
                dlg.SetNeutralButton(config.Cancel.Text, (s, a) => config.Cancel.Action?.Invoke());

            return this.SetDialogDefaults(dlg.Create(), config);
        }


        protected virtual Dialog SetDialogDefaults(Dialog dialog, ActionSheetConfig config) 
        {            
            dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            dialog.SetCancelable(config.Cancel != null);
            dialog.SetCanceledOnTouchOutside(config.Cancel != null);

            dialog.CancelEvent += (sender, args) => config.Cancel.Action.Invoke();
            dialog.DismissEvent += (sender, args) => config.Cancel.Action.Invoke();
            dialog.KeyPress += this.OnKeyPress;

            return dialog;
        }


        protected virtual void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            //this.Config?.Cancel?.Action?.Invoke();
            //this.Dismiss();
        }

        //protected virtual View GetCustomTitle(Activity activity, ActionSheetConfig config)
        //{
        //    var layout = new LinearLayout(activity)
        //    {
        //        LayoutParameters = new ViewGroup.LayoutParams(
        //            ViewGroup.LayoutParams.MatchParent,
        //            ViewGroup.LayoutParams.MatchParent
        //        )
        //    };

        //    if (!String.IsNullOrWhiteSpace(config.Message))
        //    {
        //        layout.AddView(new TextView(activity)
        //        {
        //            Text = config.Message
        //        });
        //    }

        //    var adapter = new ActionSheetListAdapter(activity, Android.Resource.Layout.SelectDialogItem, Android.Resource.Id.Text1, config);
        //    var listView = new ListView(activity)
        //    {
        //        Adapter = adapter
        //    };
        //    listView.ItemClick += (sender, args) =>
        //    {
        //        config.Options[args.Position].Action?.Invoke();
        //        // TODO: I don't have the dialog here to dismiss this.  Need to get rid of build concept!
        //    };
        //    return layout;
        //}
    }
}