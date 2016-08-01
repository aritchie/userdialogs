using System;
using Acr.UserDialogs.Builders;
using Android.Content;


namespace Acr.UserDialogs.Fragments
{
    public class ActionSheetDialogFragment : AbstractBuilderDialogFragment<ActionSheetConfig, ActionSheetBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            this.Config?.Cancel?.Action?.Invoke();
            base.OnKeyPress(sender, args);
        }
    }


    public class ActionSheetAppCompatDialogFragment : AbstractBuilderAppCompatDialogFragment<ActionSheetConfig, ActionSheetBuilder>
    {
        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            this.Config?.Cancel?.Action?.Invoke();
            base.OnKeyPress(sender, args);
        }
    }
}