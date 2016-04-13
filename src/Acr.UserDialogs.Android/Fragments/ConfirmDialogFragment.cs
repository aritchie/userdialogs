using System;
using Acr.UserDialogs.Builders;
using Android.App;


namespace Acr.UserDialogs.Fragments
{
    public class ConfirmDialogFragment : AbstractDialogFragment<ConfirmConfig>
    {
        protected override Dialog CreateDialog(ConfirmConfig config)
        {
            return ConfirmBuilder.Build(this.Activity, config).Create();
        }
    }


    public class ConfirmAppCompatDialogFragment : AbstractAppCompatDialogFragment<ConfirmConfig>
    {
        protected override Dialog CreateDialog(ConfirmConfig config)
        {
            return ConfirmBuilder.Build(this.Activity, config).Create();
        }
    }
}