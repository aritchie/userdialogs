using System;
using System.Linq;
using Android.App;
#if APPCOMPAT
using AlertDialog = Android.Support.V7.App.AlertDialog;
#else
using AlertDialog = Android.App.AlertDialog;
#endif


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
    }
}