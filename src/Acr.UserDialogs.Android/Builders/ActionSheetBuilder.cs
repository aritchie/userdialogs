using System;
using System.Linq;
using Android.App;
using Android.Support.V7.App;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;


namespace Acr.UserDialogs.Builders
{
    public static class ActionSheetBuilder
    {
        public static AlertDialog.Builder Build(Activity activity, ActionSheetConfig config)
        {
            var dlg = new AlertDialog
                .Builder(activity)
                .SetCancelable(false)
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

            return dlg;
        }


        public static AppCompatAlertDialog.Builder Build(AppCompatActivity activity, ActionSheetConfig config)
        {
            var dlg = new AppCompatAlertDialog
                .Builder(activity)
                .SetCancelable(false)
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

            return dlg;
        }
    }
}