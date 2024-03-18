using System;
using Android.App;
using AndroidX.AppCompat.App;


namespace Acr.UserDialogs.Builders
{
    public interface IAlertDialogBuilder<in TConfig>
    {
        Dialog Build(AppCompatActivity activity, TConfig config);
        Dialog Build(Activity activity, TConfig config);
    }
}