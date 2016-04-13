using System;
using Acr.UserDialogs.Builders;
using Android.App;


namespace Acr.UserDialogs.Fragments
{
    public class AlertDialogFragment : AbstractDialogFragment<AlertConfig>
    {
        protected override Dialog CreateDialog(AlertConfig config)
        {
            return AlertBuilder.Build(this.Activity, config).Create();
        }
    }
}