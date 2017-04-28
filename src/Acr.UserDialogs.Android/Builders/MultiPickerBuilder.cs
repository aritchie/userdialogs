using System;
using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using AlertDialog = Android.App.AlertDialog;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;
using System.Linq;
using Android.Content;
using Acr.UserDialogs.Builders;

namespace Acr.UserDialogs
{
	public class MultiPickerBuilder : IAlertDialogBuilder<MultiPickerPromptConfig>
	{
		public Dialog Build(Activity activity, MultiPickerPromptConfig config)
		{
			List<NumberPicker> pickers = new List<NumberPicker>();

			var layout = new LinearLayout(activity);
			layout.Orientation = Orientation.Horizontal;

			for (int i = 0; i < config.PickerCollections.Count; i++)
			{
				NumberPicker picker = new NumberPicker(activity);
				picker.MinValue = 0;
				picker.MaxValue = config.PickerCollections[i].Count - 1;
				picker.Value = config.SelectedItemIndex[i];
				picker.WrapSelectorWheel = config.IsSpinner;
				picker.SetDisplayedValues(config.PickerCollections[i].ToArray());

				// Suppress soft keyboard from the beginning
				((EditText)picker.GetChildAt(0)).SetRawInputType(InputTypes.Null);

				picker.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1);
				layout.AddView(picker);
				pickers.Add(picker);
			}

			return new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
				.SetCancelable(false)
				.SetTitle(config.Title)
				.SetView(layout)
				.SetPositiveButton(config.OkText, (s, a) =>
								   config.OnAction(new MultiPickerPromptResult(true, pickers.Select(p => p.Value).ToList()))
				)
				.SetNegativeButton(config.CancelText, (s, a) =>
								   config.OnAction(new MultiPickerPromptResult(false, pickers.Select(p => p.Value).ToList()))
				)
				.Create();
		}

		public Dialog Build(AppCompatActivity activity, MultiPickerPromptConfig config)
		{
			List<NumberPicker> pickers = new List<NumberPicker>();

			var layout = new LinearLayout(activity);
			layout.Orientation = Orientation.Horizontal;

			for (int i = 0; i < config.PickerCollections.Count; i++)
			{
				NumberPicker picker = new NumberPicker(activity);
				picker.MinValue = 0;
				picker.MaxValue = config.PickerCollections[i].Count - 1;
				picker.Value = config.SelectedItemIndex[i];
				picker.WrapSelectorWheel = config.IsSpinner;
				picker.SetDisplayedValues(config.PickerCollections[i].ToArray());

				// Suppress soft keyboard from the beginning
				((EditText)picker.GetChildAt(0)).SetRawInputType(InputTypes.Null);

				picker.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1);
				layout.AddView(picker);
				pickers.Add(picker);
			}

			return new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
				.SetCancelable(false)
				.SetTitle(config.Title)
				.SetView(layout)
				.SetPositiveButton(config.OkText, (s, a) =>
								   config.OnAction(new MultiPickerPromptResult(true, pickers.Select(p => p.Value).ToList()))
				)
				.SetNegativeButton(config.CancelText, (s, a) =>
								   config.OnAction(new MultiPickerPromptResult(false, pickers.Select(p => p.Value).ToList()))
				)
				.Create();
		}
	}
}
