using System;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;


namespace Acr.UserDialogs.Fragments
{
    public class PromptDialogFragment : AbstractDialogFragment<PromptConfig>
    {
        protected override Dialog CreateDialog(PromptConfig config)
        {
            return new PromptBuilder().Build(this.Activity, config);
        }
    }


    public class PromptAppCompatDialogFragment : AbstractAppCompatDialogFragment<PromptConfig>
    {
        protected override Dialog CreateDialog(PromptConfig config)
        {
            return new PromptBuilder().Build(this.Activity, config);
        }
    }
}