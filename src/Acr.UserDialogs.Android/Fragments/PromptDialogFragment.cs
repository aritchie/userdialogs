using System;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Support.V7.App;


namespace Acr.UserDialogs.Fragments
{
    public class PromptDialogFragment : AbstractDialogFragment<PromptConfig>
    {
        protected override Dialog CreateDialog(PromptConfig config)
        {
            return PromptBuilder.Build(this.Activity, config).Create();
        }
    }


    public class PromptAppCompatDialogFragment : AbstractAppCompatDialogFragment<PromptConfig>
    {
        protected override Dialog CreateDialog(PromptConfig config)
        {
            return PromptBuilder.Build(this.Activity as AppCompatActivity, config).Create();
        }
    }
}