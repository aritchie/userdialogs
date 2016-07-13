using System;
using Android.App;
using Android.Support.V7.App;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;


namespace Acr.UserDialogs.Builders
{
    public abstract class AbstractAlertDialogBuilder<TConfig> : IAlertDialogBuilder<TConfig>
    {
        protected AppCompatAlertDialog.Builder CreateBaseBuilder(AppCompatActivity activity, int? defaultTheme)
        {
            var builder = defaultTheme == null
                ? new AppCompatAlertDialog.Builder(activity)
                : new AppCompatAlertDialog.Builder(activity, defaultTheme.Value);

            return builder;
        }


        protected AlertDialog.Builder CreateBaseBuilder(Activity activity, int? defaultTheme)
        {
            var builder = defaultTheme == null
                ? new AlertDialog.Builder(activity)
                : new AlertDialog.Builder(activity, defaultTheme.Value);

            return builder;
        }

        public abstract AppCompatAlertDialog.Builder Build(AppCompatActivity activity, TConfig config);
        public abstract AlertDialog.Builder Build(Activity activity, TConfig config);
    }
}