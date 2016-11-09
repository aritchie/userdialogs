using System;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Content;


namespace Acr.UserDialogs.Fragments
{
    public class ActionSheetDialogFragment : AbstractDialogFragment<ActionSheetConfig>
    {
        public override void Dismiss()
        {
            base.Dismiss();
            this.Config?.Cancel?.Action?.Invoke();
        }


        public override void OnCancel(IDialogInterface dialog)
        {
            base.OnCancel(dialog);
            this.Config?.Cancel?.Action?.Invoke();
        }


        //protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        //{
        //    base.OnKeyPress(sender, args);
        //    if (args.KeyCode != Keycode.Back)
        //        return;

        //    args.Handled = true;
        //    this.Config?.Cancel?.Action?.Invoke();
        //    this.Dismiss();
        //}


        protected override Dialog CreateDialog(ActionSheetConfig config)
        {
            return new ActionSheetBuilder().Build(this.Activity, config);
        }
    }


    public class ActionSheetAppCompatDialogFragment : AbstractAppCompatDialogFragment<ActionSheetConfig>
    {
        public override void OnCancel(IDialogInterface dialog)
        {
            base.OnCancel(dialog);
            this.Config?.Cancel?.Action?.Invoke();
        }


        public override void Dismiss()
        {
            base.Dismiss();
            this.Config?.Cancel?.Action?.Invoke();
        }


        protected override Dialog CreateDialog(ActionSheetConfig config)
        {
            return new ActionSheetBuilder().Build(this.Activity, config);
        }
    }
}