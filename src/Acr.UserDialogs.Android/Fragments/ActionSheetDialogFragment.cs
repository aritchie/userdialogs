using System;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Content;
using Android.Views;


namespace Acr.UserDialogs.Fragments
{
    public class ActionSheetDialogFragment : AbstractDialogFragment<ActionSheetConfig>
    {
        protected override void SetDialogDefaults(Dialog dialog)
        {
            dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            dialog.KeyPress += this.OnKeyPress;
            dialog.CancelEvent += (sender, args) => this.Config?.Cancel?.Action?.Invoke();

            var cancellable = this.Config.Cancel != null;
            dialog.SetCancelable(cancellable);
            dialog.SetCanceledOnTouchOutside(cancellable);
        }


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


        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            this.Dismiss();
        }


        protected override Dialog CreateDialog(ActionSheetConfig config) => new ActionSheetBuilder().Build(this.Activity, config);
    }


    public class ActionSheetAppCompatDialogFragment : AbstractAppCompatDialogFragment<ActionSheetConfig>
    {
        protected override void SetDialogDefaults(Dialog dialog)
        {
            dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            dialog.KeyPress += this.OnKeyPress;
            dialog.CancelEvent += (sender, args) => this.Config?.Cancel?.Action?.Invoke();

            var cancellable = this.Config.Cancel != null;
            dialog.SetCancelable(cancellable);
            dialog.SetCanceledOnTouchOutside(cancellable);
        }


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


        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            base.OnKeyPress(sender, args);
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            this.Dismiss();
        }


        protected override Dialog CreateDialog(ActionSheetConfig config) => new ActionSheetBuilder().Build(this.AppCompatActivity, config);
    }
}