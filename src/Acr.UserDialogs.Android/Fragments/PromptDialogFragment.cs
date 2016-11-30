using System;
using System.Collections.Generic;
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

	public class PromptDialogFragmentMultiInput : AbstractDialogFragment<PromptConfigMultiInput>
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


		protected override Dialog CreateDialog(PromptConfigMultiInput config)
		{
			return new PromptBuilderMultiInput().Build(this.Activity, config);
		}


		protected virtual void SetAction(bool ok)
		{
			LinearLayout container = this.Dialog.FindViewById<LinearLayout>(Int32.MaxValue - 1000);

			List<PromptInput> inputsResult = new List<PromptInput>();
			for (int i = 0; i < container.ChildCount; i++)
			{
				EditText editText = (EditText)container.GetChildAt(i);

				PromptInput configInput = this.Config.Inputs[i];
				configInput.Text = editText.Text;

				inputsResult.Add(configInput);
			}

			this.Config?.OnAction(new PromptResultMultiInput(ok, inputsResult));
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

	public class PromptAppCompatDialogFragmentMultiInput : AbstractAppCompatDialogFragment<PromptConfigMultiInput>
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

		protected override Dialog CreateDialog(PromptConfigMultiInput config)
		{
			return new PromptBuilderMultiInput().Build(this.Activity, config);
		}


		protected virtual void SetAction(bool ok)
		{
			LinearLayout container = this.Dialog.FindViewById<LinearLayout>(Int32.MaxValue - 1000);

			List<PromptInput> inputsResult = new List<PromptInput>();
			for (int i = 0; i < container.ChildCount; i++)
			{
				EditText editText = (EditText)container.GetChildAt(i);

				PromptInput configInput = this.Config.Inputs[i];
				configInput.Text = editText.Text;

				inputsResult.Add(configInput);
			}

			this.Config?.OnAction(new PromptResultMultiInput(ok, inputsResult));
			this.Dismiss();
		}
	}

}