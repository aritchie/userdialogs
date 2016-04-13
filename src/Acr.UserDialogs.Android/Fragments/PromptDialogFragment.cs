using System;
using Acr.UserDialogs.Builders;
using Android.App;


namespace Acr.UserDialogs.Fragments
{
    public class PromptDialogFragment : AbstractDialogFragment<PromptConfig>
    {
        protected override Dialog CreateDialog(PromptConfig config)
        {
            return PromptBuilder.Build(this.Activity, config).Create();
        }
    }
}