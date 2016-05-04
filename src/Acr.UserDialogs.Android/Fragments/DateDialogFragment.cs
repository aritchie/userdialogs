using System;
using Acr.UserDialogs.Builders;
using Android.App;


namespace Acr.UserDialogs.Fragments
{
    public class DateDialogFragment : AbstractDialogFragment<DatePromptConfig>
    {
        protected override Dialog CreateDialog(DatePromptConfig config)
        {
            return DatePromptBuilder.Build(this.Activity, config);
        }
    }


    public class DateAppCompatDialogFragment : AbstractAppCompatDialogFragment<DatePromptConfig>
    {
        protected override Dialog CreateDialog(DatePromptConfig config)
        {
            return DatePromptBuilder.Build(this.Activity, config);
        }
    }
}