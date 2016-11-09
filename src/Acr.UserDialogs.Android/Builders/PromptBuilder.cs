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
    public class PromptBuilder
    {
        public Dialog Build(Activity activity, PromptConfig config)
        {
            var txt = new EditText(activity)
            {
                Id = Int32.MaxValue,
                Hint = config.Placeholder
            };
            if (config.Text != null)
                txt.Text = config.Text;

            if (config.MaxLength != null)
                txt.SetFilters(new [] { new InputFilterLengthFilter(config.MaxLength.Value) });

            SetInputType(txt, config.InputType);

            var builder = new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetView(txt)
                .SetPositiveButton(config.Positive.Text, (s, a) =>
                    config.OnAction(new DialogResult<string>(DialogChoice.Positive, txt.Text.Trim()))
                );

            if (config.Neutral.IsVisible)
            {
                builder.SetNeutralButton(config.Neutral.Text, (s, a) =>
                    config.OnAction(new DialogResult<string>(DialogChoice.Neutral, txt.Text.Trim()))
                );
            }
            if (config.Negative.IsVisible)
            {
                builder.SetNegativeButton(config.Negative.Text, (s, a) =>
                    config.OnAction(new DialogResult<string>(DialogChoice.Negative, txt.Text.Trim()))
                );
            }
            var dialog = builder.Create();
            this.HookTextChanged(dialog, txt, config.OnTextChanged);

            return dialog;
        }


        public Dialog Build(AppCompatActivity activity, PromptConfig config)
        {
            var txt = new EditText(activity)
            {
                Id = Int32.MaxValue,
                Hint = config.Placeholder
            };
            if (config.Text != null)
                txt.Text = config.Text;

            SetInputType(txt, config.InputType);

            var builder = new AppCompatAlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetView(txt)
                .SetPositiveButton(config.Positive.Text, (s, a) =>
                    config.OnAction(new DialogResult<string>(DialogChoice.Positive, txt.Text.Trim()))
                );

            if (config.Neutral.IsVisible)
            {
                builder.SetNeutralButton(config.Neutral.Text, (s, a) =>
                    config.OnAction(new DialogResult<string>(DialogChoice.Neutral, txt.Text.Trim()))
                );
            }
            if (config.Negative.IsVisible)
            {
                builder.SetNegativeButton(config.Negative.Text, (s, a) =>
                    config.OnAction(new DialogResult<string>(DialogChoice.Negative, txt.Text.Trim()))
                );
            }
            var dialog = builder.Create();
            this.HookTextChanged(dialog, txt, config.OnTextChanged);

            return dialog;
        }


        protected virtual void HookTextChanged(Dialog dialog, EditText txt, Action<PromptTextChangedArgs> onChange)
        {
            if (onChange == null)
                return;

            var buttonId = (int) Android.Content.DialogButtonType.Positive;
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


        //protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        //{
        //    base.OnKeyPress(sender, args);
        //    args.Handled = false;

        //    switch (args.KeyCode)
        //    {
        //        case Keycode.Back:
        //            args.Handled = true;
        //            if (this.Config.IsCancellable)
        //                this.SetAction(false);
        //        break;

        //        case Keycode.Enter:
        //            args.Handled = true;
        //            this.SetAction(true);
        //        break;
        //    }
        //}


        //protected virtual void SetAction(bool ok)
        //{
        //    var txt = this.Dialog.FindViewById<TextView>(Int32.MaxValue);
        //    this.Config?.OnAction(new PromptResult(ok, txt.Text.Trim()));
        //    this.Dismiss();
        //}


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