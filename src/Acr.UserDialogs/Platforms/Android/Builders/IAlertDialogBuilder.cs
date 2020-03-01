using System;
using Android.App;
#if ANDROIDX
using AndroidX.AppCompat.App;
#else
using Android.Support.V7.App;
#endif

namespace Acr.UserDialogs.Builders
{
    public interface IAlertDialogBuilder<in TConfig>
    {
        Dialog Build(AppCompatActivity activity, TConfig config);
        Dialog Build(Activity activity, TConfig config);
    }
}