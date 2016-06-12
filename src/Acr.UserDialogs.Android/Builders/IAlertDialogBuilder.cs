using System;
using Android.App;
using Android.Support.V7.App;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;


namespace Acr.UserDialogs.Builders
{
    public interface IAlertDialogBuilder<in TConfig>
    {
        AppCompatAlertDialog.Builder Build(AppCompatActivity activity, TConfig config);
        AlertDialog.Builder Build(Activity activity, TConfig config);
    }
}