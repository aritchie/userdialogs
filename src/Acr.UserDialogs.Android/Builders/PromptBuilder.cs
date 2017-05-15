using System;
using System.Globalization;
using Android.App;
using Android.Content;
using Android.Support.V7.App;
using Android.Text;
using Android.Text.Method;
using Android.Views.InputMethods;
using Android.Widget;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;


namespace Acr.UserDialogs.Builders
{
    public class PromptBuilder : IAlertDialogBuilder<PromptConfig>
    {
        public Dialog Build(Activity activity, PromptConfig config)
        {
            var txt = new EditText(activity)
            {
                Id = Int32.MaxValue,
                Hint = config.Placeholder
            };
            if (config.Text != null)
            {
                txt.Text = config.Text;
                txt.SetSelection(config.Text.Length);
            }

            if (config.MaxLength != null)
                txt.SetFilters(new [] { new InputFilterLengthFilter(config.MaxLength.Value) });

            SetInputType(txt, config.InputType);

            var builder = new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetView(txt)
                .SetPositiveButton(config.OkText, (s, a) =>
                    config.OnAction(new PromptResult(true, txt.Text.Trim()))
                );

            if (config.IsCancellable)
            {
                builder.SetNegativeButton(config.CancelText, (s, a) =>
                    config.OnAction(new PromptResult(false, txt.Text.Trim()))
                );
            }
            var dialog = builder.Create();
            this.HookTextChanged(dialog, txt, config);

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
            {
                txt.Text = config.Text;
                txt.SetSelection(config.Text.Length);
            }
            if (config.MaxLength != null)
                txt.SetFilters(new[] { new InputFilterLengthFilter(config.MaxLength.Value) });

            SetInputType(txt, config.InputType);

            var builder = new AppCompatAlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetView(txt)
                .SetPositiveButton(config.OkText, (s, a) =>
                    config.OnAction(new PromptResult(true, txt.Text.Trim()))
                );

            if (config.IsCancellable)
            {
                builder.SetNegativeButton(config.CancelText, (s, a) =>
                    config.OnAction(new PromptResult(false, txt.Text.Trim()))
                );
            }
            var dialog = builder.Create();
            this.HookTextChanged(dialog, txt, config);

            return dialog;
        }


        protected virtual void HookTextChanged(Dialog dialog, EditText txt, PromptConfig config)
        {
            if (config.OnTextChanged == null)
                return;

            // HACK: this is a temporary fix to deal with restarting input and causing the result action to fire
            // this will at least block completion of your prompt via the soft keyboard
            txt.ImeOptions = ImeAction.None;
            var buttonId = (int)DialogButtonType.Positive;
            var promptArgs = new PromptTextChangedArgs { Value = txt.Text };

            dialog.ShowEvent += (sender, args) =>
            {
                config.OnTextChanged(promptArgs);
                this.GetButton(dialog, buttonId).Enabled = promptArgs.IsValid;
                //this.ChangeImeOption(config, txt, promptArgs.IsValid);
            };
            txt.AfterTextChanged += (sender, args) =>
            {
                promptArgs.IsValid = true;
                promptArgs.Value = txt.Text;
                config.OnTextChanged(promptArgs);
                this.GetButton(dialog, buttonId).Enabled = promptArgs.IsValid;
                //this.ChangeImeOption(config, txt, promptArgs.IsValid);

                if (!txt.Text.Equals(promptArgs.Value))
                {
                    txt.Text = promptArgs.Value;
                    txt.SetSelection(txt.Text.Length);
                }
            };
        }


        //protected virtual void ChangeImeOption(PromptConfig config, EditText txt, bool enable)
        //{
            //var action = enable ? ImeAction.Done : ImeAction.None;
            //if (txt.ImeOptions == action)
            //    return;

            //txt.ImeOptions = action;
            //var input = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);
            //input.RestartInput(txt);
        //}


        protected virtual Button GetButton(Dialog dialog, int buttonId)
        {
            var ac = dialog as AppCompatAlertDialog;
            if (ac != null)
                return ac.GetButton(buttonId);

            var old = dialog as AlertDialog;
            if (old != null)
                return old.GetButton(buttonId);

            throw new ArgumentException("Invalid dialog type.  This exception should never be seen. " + dialog.GetType());
        }


        public static void SetInputType(TextView txt, InputType inputType)
        {
            txt.SetSingleLine(true);
            switch (inputType)
            {
                case InputType.DecimalNumber:
                    txt.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal | InputTypes.NumberFlagSigned;
                    txt.SetSingleLine(true);
                    txt.KeyListener = DigitsKeyListener.GetInstance("1234567890" + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
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
                    txt.TransformationMethod = PasswordTransformationMethod.Instance;
                    txt.InputType = InputTypes.ClassNumber;
                    break;

                case InputType.Password:
                    txt.TransformationMethod = PasswordTransformationMethod.Instance;
                    //txt.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
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