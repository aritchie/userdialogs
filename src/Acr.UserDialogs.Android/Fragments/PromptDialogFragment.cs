using System;
using Acr.UserDialogs.Builders;
using Android.Content;


namespace Acr.UserDialogs.Fragments
{
    public class PromptDialogFragment : AbstractBuilderDialogFragment<PromptConfig, PromptBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            this.Config?.OnResult(new PromptResult(false, null));
            base.OnKeyPress(sender, args);
        }
    }


    public class PromptAppCompatDialogFragment : AbstractBuilderAppCompatDialogFragment<PromptConfig, PromptBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            this.Config?.OnResult(new PromptResult(false, null));
            base.OnKeyPress(sender, args);
        }
    }
}