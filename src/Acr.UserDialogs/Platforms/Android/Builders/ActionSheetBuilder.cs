using System;
using System.Linq;
using Android.App;
using Android.Support.V7.App;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;
using Acr.UserDialogs.Infrastructure;

namespace Acr.UserDialogs.Builders
{
    public class ActionSheetBuilder : IAlertDialogBuilder<ActionSheetConfig>
    {
        public Dialog Build(Activity activity, ActionSheetConfig config)
        {
            ShowWarningsIfAny(config);

            var dlg = new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetTitle(config.Title)
                .SetMessage(config.Message);
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

            return dlg.Create();
        }


        public Dialog Build(AppCompatActivity activity, ActionSheetConfig config)
        {
            ShowWarningsIfAny(config);

            var dlg = new AppCompatAlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetTitle(config.Title)
                .SetMessage(config.Message);
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

            return dlg.Create();
        }

        void ShowWarningsIfAny(ActionSheetConfig config)
        {
            var hasMessage = !string.IsNullOrEmpty(config.Message);
            var hasOptions = config.Options.Any() || config.ItemIcon != null;
            if (hasMessage && hasOptions)
            {
                var warning = 
@"ActionSheet doesn't not support using both Message and Items on Android.
Use either one or another. Currently only Message is present.";
                Log.Warn("", warning);
            }
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