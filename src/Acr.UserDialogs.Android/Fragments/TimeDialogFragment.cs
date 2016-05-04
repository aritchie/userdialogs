using System;
using Acr.UserDialogs.Builders;
using Android.App;


namespace Acr.UserDialogs.Fragments
{
    public class TimeDialogFragment : AbstractDialogFragment<TimePromptConfig>
    {
        protected override Dialog CreateDialog(TimePromptConfig config)
        {
            return TimePromptBuilder.Build(this.Activity, config);
        }
    }


    public class TimeAppCompatDialogFragment : AbstractAppCompatDialogFragment<TimePromptConfig>
    {
        protected override Dialog CreateDialog(TimePromptConfig config)
        {
            return TimePromptBuilder.Build(this.Activity, config);
        }
    }
}