using System;
using Android.App;
using Android.Support.V7.App;
using Android.Widget;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;

namespace Acr.UserDialogs.Builders
{
    public class NumberPromptBuilder : IAlertDialogBuilder<NumberPromptConfig>
    {
        public Dialog Build(Activity activity, NumberPromptConfig config)
        {
            NumberPicker numberPicker = new NumberPicker(activity);
            AlertDialog.Builder dialog = new AlertDialog
                .Builder(activity, config.AndroidStyleId ?? 0)
                .SetView(numberPicker);


            dialog.SetCancelable(config.IsCancellable);

            if (!String.IsNullOrWhiteSpace(config.Title))
                dialog.SetTitle(config.Title);

            if (config.MinNumber != null)
            {
                numberPicker.MinValue = config.MinNumber.Value;
            }

            if (config.MaxNumber != null)
            {
                numberPicker.MaxValue = config.MaxNumber.Value;
            }

            if (config.SelectedNumber != null)
            {
                numberPicker.Value = config.SelectedNumber.Value;
            }

            if (config.IsCancellable)
            {
                dialog.SetNegativeButton(config.CancelText, (s, a) => config?.OnAction?.Invoke(new NumberPromptResult(false, 0)));
            }

            dialog.SetPositiveButton(config.OkText, (s, a) => config?.OnAction?.Invoke(new NumberPromptResult(true, numberPicker.Value)));

            return dialog.Show();
        }

        public Dialog Build(AppCompatActivity activity, NumberPromptConfig config)
        {
            NumberPicker numberPicker = new NumberPicker(activity);
            AppCompatAlertDialog.Builder dialog = new AppCompatAlertDialog
                .Builder(activity, config.AndroidStyleId ?? 0)
                .SetView(numberPicker);


            dialog.SetCancelable(config.IsCancellable);

            if (!String.IsNullOrWhiteSpace(config.Title))
                dialog.SetTitle(config.Title);

            if (config.MinNumber != null)
            {
                numberPicker.MinValue = config.MinNumber.Value;
            }

            if (config.MaxNumber != null)
            {
                numberPicker.MaxValue = config.MaxNumber.Value;
            }

            if (config.SelectedNumber != null)
            {
                numberPicker.Value = config.SelectedNumber.Value;
            }

            if (config.IsCancellable)
            {
                dialog.SetNegativeButton(config.CancelText, (s, a) => config?.OnAction?.Invoke(new NumberPromptResult(false, 0)));
            }

            dialog.SetPositiveButton(config.OkText, (s, a) => config?.OnAction?.Invoke(new NumberPromptResult(true, numberPicker.Value)));


            return dialog.Create();
        }
    }
}
