using System;
using Android.App;
using Android.Support.V7.App;
using Android.Text;
using Android.Text.Method;
using Android.Widget;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;


namespace Acr.UserDialogs.Builders
{
    public static class PromptBuilder
    {
        public static AlertDialog.Builder Build(Activity activity, PromptConfig config)
        {
            var txt = new EditText(activity)
            {
                Hint = config.Placeholder
            };
            if (config.Text != null)
                txt.Text = config.Text;

            SetInputType(txt, config.InputType);

            var builder = new AlertDialog
                .Builder(activity)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetView(txt)
                .SetPositiveButton(config.OkText, (s, a) =>
                    config.OnResult(new PromptResult(true, txt.Text.Trim()))
                );

            if (config.IsCancellable)
            {
                builder.SetNegativeButton(config.CancelText, (s, a) =>
                    config.OnResult(new PromptResult(false, txt.Text.Trim()))
                );
            }
            return builder;
        }

		// PromptTwoInputs added by Lee Bettridge
		public static AppCompatAlertDialog.Builder Build(AppCompatActivity activity, PromptConfig config)
        {
			EditText txtOne = null, txtTwo = null;
			AppCompatAlertDialog.Builder builder = null;

			txtOne = new EditText(activity)
			{
				Hint = config.Placeholder
			};

			if (config.Text != null)
				txtOne.Text = config.Text;

			SetInputType(txtOne, config.InputType);

			if (config.ShowSecondInput == false)
			{
				builder = new AppCompatAlertDialog
					.Builder(activity)
					.SetCancelable(false)
					.SetMessage(config.Message)
					.SetTitle(config.Title)
					.SetView(txtOne)
					.SetPositiveButton(config.OkText, (s, a) =>
						config.OnResult(new PromptResult(true, txtOne.Text.Trim()))
				);

				if (config.IsCancellable)
				{
					builder.SetNegativeButton(config.CancelText, (s, a) =>
						config.OnResult(new PromptResult(false, txtOne.Text.Trim()))
					);
				}
			}
			else
			{
				txtTwo = new EditText(activity)
				{
					Hint = config.SecondPlaceholder
				};

				SetInputType(txtTwo, config.SecondInputType);

				LinearLayout layout = new LinearLayout(activity.ApplicationContext);
				layout.Orientation = Orientation.Vertical;
				layout.AddView(txtOne);
				layout.AddView(txtTwo);

				builder = new AppCompatAlertDialog
					.Builder(activity)
					.SetCancelable(false)
					.SetMessage(config.Message)
					.SetTitle(config.Title)
					.SetView(layout)
					.SetPositiveButton(config.OkText, (s, a) =>
						config.OnResult(new PromptResult(true, txtOne.Text.Trim(), txtTwo.Text.Trim()))
				);

				if (config.IsCancellable)
				{
					builder.SetNegativeButton(config.CancelText, (s, a) =>
						config.OnResult(new PromptResult(false, txtOne.Text.Trim(), txtTwo.Text.Trim()))
					);
				}
			}

            return builder;
        }


        public static void SetInputType(TextView txt, InputType inputType)
        {
            switch (inputType)
            {
                case InputType.DecimalNumber:
                    txt.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;
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