using System;
namespace Acr.UserDialogs
{
    public class NumberPromptConfig : IAndroidStyleDialogConfig
    {
        public static string DefaultOkText { get; set; } = "Ok";
        public static string DefaultCancelText { get; set; } = "Cancel";
        public static int? DefaultAndroidStyleId { get; set; }


        public string Title { get; set; }
        public string OkText { get; set; } = DefaultOkText;
        public string CancelText { get; set; } = DefaultCancelText;
        public int? SelectedNumber { get; set; }


        public Action<NumberPromptResult> OnAction { get; set; }
        public bool IsCancellable { get; set; } = true;

        public int? MinNumber { get; set; }
        public int? MaxNumber { get; set; }
        public int? AndroidStyleId { get; set; } = DefaultAndroidStyleId;
    }
}
