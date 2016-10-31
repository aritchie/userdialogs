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


        protected override Dialog CreateDialog(PromptConfig config)
        {
            return new PromptBuilder().Build(this.Activity, config);
        }


        protected virtual void SetAction(bool ok)
        {
            var txt = this.Dialog.FindViewById<TextView>(Int32.MaxValue);
            this.Config?.OnAction(new PromptResult(ok, txt.Text.Trim()));
            this.Dismiss();
        }
    }


    public class PromptAppCompatDialogFragment : AbstractAppCompatDialogFragment<PromptConfig>
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

        protected override Dialog CreateDialog(PromptConfig config)
        {
            return new PromptBuilder().Build(this.Activity, config);
        }


        protected virtual void SetAction(bool ok)
        {
            var txt = this.Dialog.FindViewById<TextView>(Int32.MaxValue);
            this.Config?.OnAction(new PromptResult(ok, txt.Text.Trim()));
            this.Dismiss();
        }
    }
}