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
            switch (args.KeyCode)
            {
                case Keycode.Back:
                case Keycode.Enter:
                    args.Handled = true;
                    var ok = args.KeyCode == Keycode.Enter;
                    this.Config?.OnAction(new PromptResult(ok, ""));
                    this.Dismiss();
                    break;
            }
            base.OnKeyPress(sender, args);
       }
    }


    public class PromptAppCompatDialogFragment : AbstractBuilderAppCompatDialogFragment<PromptConfig, PromptBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
        }
    }
}