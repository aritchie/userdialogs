using System;


namespace Acr.UserDialogs
{
    using System.Collections;
    using System.Collections.Generic;

    public class MultiPickerPromptConfig
    {
        public static string DefaultOkText { get; set; } = "Ok";
        public static string DefaultCancelText { get; set; } = "Cancel";

        public string Title { get; set; }
        public string OkText { get; set; } = DefaultOkText;
        public string CancelText { get; set; } = DefaultCancelText;
        public IList<IList<string>> PickerCollections { get; set; }
        public IList<int> SelectedItemIndex { get; set; }
        public bool IsSpinner { get; set; } = false;

        public Action<MultiPickerPromptResult> OnAction { get; set; }
        public bool IsCancellable { get; set; } = true;
    }
}
