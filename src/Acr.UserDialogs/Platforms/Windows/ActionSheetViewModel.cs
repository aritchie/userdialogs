using System;
using System.Collections.Generic;


namespace Acr.UserDialogs
{

    public class ActionSheetViewModel
    {

        public string Title { get; set; }
        public string Message { get; set; }

        public Visibility MessageVisibility
            => String.IsNullOrWhiteSpace(this.Message) ? Visibility.Collapsed : Visibility.Visible;

        public ActionSheetOptionViewModel Destructive { get; set; }
        public Visibility DestructiveVisibility { get; set; }

        public ActionSheetOptionViewModel Cancel { get; set; }
        public Visibility CancelVisibility { get; set; }

        public IList<ActionSheetOptionViewModel> Options { get; set; }
    }
}
