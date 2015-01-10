using System;
using System.Collections.Generic;


namespace Acr.UserDialogs {
    
    public class ActionSheetConfig {

        public string Title { get; set; }
        //public SheetOption Cancel { get; set; }
        public IList<ActionSheetOption> Options { get; set; }


        public ActionSheetConfig() {
            this.Options = new List<ActionSheetOption>();
        }


        public ActionSheetConfig SetTitle(string title) {
            this.Title = title;
            return this;
        }


        //public ActionSheetOptions SetCancel(string text = "Cancel", Action action = null) {
        //    this.Cancel = new SheetOption(text, action);
        //    return this;
        //}


        public ActionSheetConfig Add(string text, Action action = null) {
            this.Options.Add(new ActionSheetOption(text, action));
            return this;
        }
    }
}
