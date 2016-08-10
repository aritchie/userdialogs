using System;
using Acr.UserDialogs.Builders;
using Android.Content;
using Android.Views;
using Android.Widget;


namespace Acr.UserDialogs.Fragments
{
    public class PromptDialogFragment : AbstractBuilderDialogFragment<PromptConfig, PromptBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            args.Handled = false;

            switch (args.KeyCode)
            {
                case Keycode.Back:
                    args.Handled = true;
                    if (this.Config.IsCancellable)
                        this.SetAction(false);
                    break;

                case Keycode.Enter:
                    args.Handled = true;
                    this.SetAction(true);
                    break;
            }
        }


        protected virtual void SetAction(bool ok)
        {
            var txt = this.Dialog.FindViewById<TextView>(Int32.MaxValue);
            this.Config?.OnAction(new PromptResult(ok, txt.Text.Trim()));
            this.Dismiss();
        }
    }


    public class PromptAppCompatDialogFragment : AbstractBuilderAppCompatDialogFragment<PromptConfig, PromptBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            args.Handled = false;

            switch (args.KeyCode)
            {
                case Keycode.Back:
                    args.Handled = true;
                    if (this.Config.IsCancellable)
                        this.SetAction(false);
                    break;

                case Keycode.Enter:
                    args.Handled = true;
                    this.SetAction(true);
                    break;
            }
        }


        protected virtual void SetAction(bool ok)
        {
            var txt = this.Dialog.FindViewById<TextView>(Int32.MaxValue);
            this.Config?.OnAction(new PromptResult(ok, txt.Text.Trim()));
            this.Dismiss();
        }
    }
}