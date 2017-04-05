using System;
using Acr.UserDialogs.Builders;
using Android.App;
using Android.Content;
using Android.Views;


namespace Acr.UserDialogs.Fragments
{
	public class MultiPickerDialogFragment : AbstractDialogFragment<MultiPickerPromptConfig>
	{
		protected override Dialog CreateDialog(MultiPickerPromptConfig config)
		{
			return new MultiPickerBuilder().Build(this.Activity, config);
		}
	}

	public class MultiPickerAppCompatDialogFragment : AbstractAppCompatDialogFragment<MultiPickerPromptConfig>
	{
		protected override Dialog CreateDialog(MultiPickerPromptConfig config)
		{
			return new MultiPickerBuilder().Build(this.AppCompatActivity, config);
		}
	}
}