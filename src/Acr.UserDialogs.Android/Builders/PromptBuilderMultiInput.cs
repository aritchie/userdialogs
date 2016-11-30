using System;
using System.Collections.Generic;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Support.V7.App;
using Android.Text;
using Android.Text.Method;
using Android.Widget;
using Java.Util;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;

namespace Acr.UserDialogs.Builders
{
	public class PromptBuilderMultiInput : IAlertDialogBuilder<PromptConfigMultiInput>
	{

		private Activity _activity;

		public Dialog Build(Activity activity, PromptConfigMultiInput config)
		{
			_activity = activity;

			LinearLayout container = new LinearLayout(activity);
			container.Id = Int32.MaxValue - 1000;
			container.Orientation = Orientation.Vertical;
			foreach (PromptInput item in config.Inputs)
			{
				var editText = new EditText(activity)
				{
					Id = Int32.MaxValue,
					Hint = item.Placeholder
				};

				SetInputType(editText, item.InputType);

				container.AddView(editText);
			}



			var builder = new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
				.SetCancelable(false)
				.SetMessage(config.Message)
				.SetTitle(config.Title)
				.SetView(container)
				.SetPositiveButton(config.OkText, (s, a) =>
					ExecutePromptResultMultiInput(config, container)
				);

			if (config.IsCancellable)
			{
				builder.SetNegativeButton(config.CancelText, (s, a) =>
					config.OnAction(new PromptResultMultiInput(false, config.Inputs))
				);
			}
			var dialog = builder.Create();
			//this.HookTextChanged(dialog, txt, config.OnTextChanged);

			return dialog;
		}

		private void ExecutePromptResultMultiInput(PromptConfigMultiInput config, LinearLayout container) 
		{
			List<PromptInput> inputsResult = new List<PromptInput>();
			for (int i = 0; i < container.ChildCount; i++)
			{
				EditText editText = (EditText)container.GetChildAt(i);

				PromptInput configInput = config.Inputs[i];
				configInput.Text = editText.Text;

				inputsResult.Add(configInput);
			}

			config.OnAction(new PromptResultMultiInput(true, inputsResult));
		}

		public Dialog Build(AppCompatActivity activity, PromptConfigMultiInput config)
		{
			_activity = activity;

			LinearLayout container = new LinearLayout(activity);
			container.Id = Int32.MaxValue - 1000;
			container.Orientation = Orientation.Vertical;

			foreach (PromptInput item in config.Inputs)
			{
				var editText = new EditText(activity)
				{
					Id = Int32.MaxValue,
					Hint = item.Placeholder
				};

				SetInputType(editText, item.InputType);

				container.AddView(editText);
			}

			var builder = new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
				.SetCancelable(false)
				.SetMessage(config.Message)
				.SetTitle(config.Title)
				.SetView(container)
				.SetPositiveButton(config.OkText, (s, a) =>
					ExecutePromptResultMultiInput(config, container)
				);

			if (config.IsCancellable)
			{
				builder.SetNegativeButton(config.CancelText, (s, a) =>
					config.OnAction(new PromptResultMultiInput(false, config.Inputs))
				);
			}
			var dialog = builder.Create();
			//this.HookTextChanged(dialog, txt, config.OnTextChanged);

			return dialog;
		}


		protected virtual void HookTextChanged(Dialog dialog, EditText txt, Action<PromptTextChangedArgs> onChange)
		{
			if (onChange == null)
				return;

			var buttonId = (int)Android.Content.DialogButtonType.Positive;
			var promptArgs = new PromptTextChangedArgs { Value = String.Empty };

			dialog.ShowEvent += (sender, args) =>
			{
				onChange(promptArgs);
				((AlertDialog)dialog).GetButton(buttonId).Enabled = promptArgs.IsValid;
			};
			txt.AfterTextChanged += (sender, args) =>
			{
				promptArgs.IsValid = true;
				promptArgs.Value = txt.Text;
				onChange(promptArgs);
				((AlertDialog)dialog).GetButton(buttonId).Enabled = promptArgs.IsValid;

				if (!txt.Text.Equals(promptArgs.Value))
				{
					txt.Text = promptArgs.Value;
					txt.SetSelection(txt.Text.Length);
				}
			};
		}


		public static void SetInputType(TextView txt, InputType inputType)
		{
			switch (inputType)
			{
				case InputType.DecimalNumber:
					txt.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal | InputTypes.NumberFlagSigned;
					txt.SetSingleLine(true);
					break;

				case InputType.Email:
					txt.InputType = InputTypes.ClassText | InputTypes.TextVariationEmailAddress;
					txt.SetSingleLine(true);
					break;

				case InputType.Name:
					txt.InputType = InputTypes.TextVariationPersonName;
					txt.SetSingleLine(true);
					break;

				case InputType.Number:
					txt.InputType = InputTypes.ClassNumber;
					txt.SetSingleLine(true);
					break;

				case InputType.NumericPassword:
					txt.InputType = InputTypes.ClassNumber;
					txt.TransformationMethod = PasswordTransformationMethod.Instance;
					break;

				case InputType.Password:
					txt.TransformationMethod = PasswordTransformationMethod.Instance;
					txt.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
					break;

				case InputType.Phone:
					txt.InputType = InputTypes.ClassPhone;
					txt.SetSingleLine(true);
					break;

				case InputType.Url:
					txt.InputType = InputTypes.TextVariationUri;
					txt.SetSingleLine(true);
					break;
			}
		}

	}
}
