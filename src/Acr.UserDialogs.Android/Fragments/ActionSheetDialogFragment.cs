using System;
using Acr.UserDialogs.Builders;
using Android.Content;
using Android.Views;


namespace Acr.UserDialogs.Fragments
{
    public class ActionSheetDialogFragment : AbstractBuilderDialogFragment<ActionSheetConfig, ActionSheetBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            this.Config?.Cancel?.Action?.Invoke();
            this.Dismiss();
        }
    }


    public class ActionSheetAppCompatDialogFragment : AbstractBuilderAppCompatDialogFragment<ActionSheetConfig, ActionSheetBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            this.Config?.Cancel?.Action?.Invoke();
            this.Dismiss();
        }
    }
}