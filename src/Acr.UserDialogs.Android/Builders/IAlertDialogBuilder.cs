using System;
using Android.App;
using Android.Support.V7.App;


namespace Acr.UserDialogs.Builders
{
    public interface IAlertDialogBuilder<in TConfig>
    {
        Dialog Build(AppCompatActivity activity, TConfig config);
        Dialog Build(Activity activity, TConfig config);
    }
}