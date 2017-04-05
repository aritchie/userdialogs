using System;
using Acr.UserDialogs.Builders;
using Android.Content;
using Android.Views;


namespace Acr.UserDialogs.Fragments
{
	public class MultiPickerDialogFragment : AbstractBuilderDialogFragment<MultiPickerPromptConfig, MultiPickerBuilder>
	{
		protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
		{
			base.OnKeyPress(sender, args);
			if (args.KeyCode != Keycode.Back)
				return;

			this.Config?.OnAction(new MultiPickerPromptResult(false, null));
			this.Dismiss();
		}
	}


	public class MultiPickerAppCompatDialogFragment : AbstractBuilderAppCompatDialogFragment<MultiPickerPromptConfig, MultiPickerBuilder>
	{
		protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
		{
			base.OnKeyPress(sender, args);
			if (args.KeyCode != Keycode.Back)
				return;

			this.Config?.OnAction(new MultiPickerPromptResult(false, null));
			this.Dismiss();
		}
	}
}