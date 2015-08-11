using System;
using System.Collections.Generic;
using Windows.UI.Xaml;


namespace Acr.UserDialogs {

    public class ActionSheetViewModel {

        public string Title { get; set; }

        public ActionSheetOptionViewModel Destructive { get; set; }
        public Visibility DestructiveVisibility { get; set; }

        public ActionSheetOptionViewModel Cancel { get; set; }
        public Visibility CancelVisibility { get; set; }

        public IList<ActionSheetOptionViewModel> Options { get; set; }
    }
}
