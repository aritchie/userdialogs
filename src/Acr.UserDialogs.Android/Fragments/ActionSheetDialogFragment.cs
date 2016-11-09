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
            if (this.Config.Cancel == null)
            {
                dialog.SetCancelable(false);
                dialog.SetCanceledOnTouchOutside(false);
            }
            else
            {
                dialog.SetCancelable(true);
                dialog.SetCanceledOnTouchOutside(true);
                dialog.CancelEvent += (sender, args) => this.Config.Cancel.Action.Invoke();
            }
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
            this.Config?.Cancel?.Action?.Invoke();
            this.Dismiss();
        }

        protected override Dialog CreateDialog(ActionSheetConfig config)
        {
            return new ActionSheetBuilder().Build(this.Activity, config);
        }
    }


    public class ActionSheetAppCompatDialogFragment : AbstractAppCompatDialogFragment<ActionSheetConfig>
    {
        protected override void SetDialogDefaults(Dialog dialog)
        {
            dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            dialog.KeyPress += this.OnKeyPress;
            if (this.Config.Cancel == null)
            {
                dialog.SetCancelable(false);
                dialog.SetCanceledOnTouchOutside(false);
            }
            else
            {
                dialog.SetCancelable(true);
                dialog.SetCanceledOnTouchOutside(true);
                dialog.CancelEvent += (sender, args) => this.Config.Cancel.Action.Invoke();
            }
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
            this.Config?.Cancel?.Action?.Invoke();
            this.Dismiss();
        }


        protected override Dialog CreateDialog(ActionSheetConfig config)
        {
            return new ActionSheetBuilder().Build(this.Activity, config);
        }
    }
}