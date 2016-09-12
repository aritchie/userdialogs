using System;


namespace Acr.UserDialogs
{
    using System.Collections;
    using System.Collections.Generic;

    public class PickerPromptConfig
    {
        public static string DefaultOkText { get; set; } = "Ok";
        public static string DefaultCancelText { get; set; } = "Cancel";

        public string Title { get; set; }
        public string OkText { get; set; } = DefaultOkText;
        public string CancelText { get; set; } = DefaultCancelText;
        public IList<IList<string>> PickerCollections { get; set; }
        public IList<int> SelectedItemIndex { get; set; }

        public Action<PickerPromptResult> OnAction { get; set; }
        public bool IsCancellable { get; set; } = true;
    }
}
