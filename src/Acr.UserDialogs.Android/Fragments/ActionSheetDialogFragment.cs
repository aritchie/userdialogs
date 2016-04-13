using System;
using Acr.UserDialogs.Builders;
using Android.App;



namespace Acr.UserDialogs.Fragments
{
    public class ActionSheetDialogFragment : AbstractDialogFragment<ActionSheetConfig>
    {
        protected override Dialog CreateDialog(ActionSheetConfig config)
        {
            return ActionSheetBuilder.Build(this.Activity, config).Create();
        }
    }


    public class ActionSheetAppCompatDialogFragment : AbstractAppCompatDialogFragment<ActionSheetConfig>
    {
        protected override Dialog CreateDialog(ActionSheetConfig config)
        {
            return ActionSheetBuilder.Build(this.Activity, config).Create();
        }
    }
}