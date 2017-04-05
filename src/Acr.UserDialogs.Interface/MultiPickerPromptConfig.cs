using System;


namespace Acr.UserDialogs
{
	using System.Collections;
	using System.Collections.Generic;

	public class MultiPickerPromptConfig
	{
		public static string DefaultTitle { get; set; } = "Multi Picker";
		public static string DefaultOkText { get; set; } = "Ok";
		public static string DefaultCancelText { get; set; } = "Cancel";
		public static int? DefaultAndroidStyleId { get; set; }

		public string Title { get; set; } = DefaultTitle;
		public string OkText { get; set; } = DefaultOkText;
		public string CancelText { get; set; } = DefaultCancelText;
		public int? AndroidStyleId { get; set; } = DefaultAndroidStyleId;

		public IList<IList<string>> PickerCollections { get; set; }
		public IList<int> SelectedItemIndex { get; set; }
		public bool IsSpinner { get; set; } = false;

		public Action<MultiPickerPromptResult> OnAction { get; set; }
		public bool IsCancellable { get; set; } = true;
	}
}
