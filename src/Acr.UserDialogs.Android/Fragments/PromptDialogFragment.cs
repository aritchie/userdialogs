using System;
using Acr.UserDialogs.Builders;
using Android.Content;
using Android.Views;


namespace Acr.UserDialogs.Fragments
{
    public class PromptDialogFragment : AbstractBuilderDialogFragment<PromptConfig, PromptBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            this.Config?.OnResult?.Invoke(new PromptResult(false, null));
            this.Dismiss();
        }
    }


    public class PromptAppCompatDialogFragment : AbstractBuilderAppCompatDialogFragment<PromptConfig, PromptBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            this.Config?.OnResult?.Invoke(new PromptResult(false, null));
            this.Dismiss();
        }
    }
}