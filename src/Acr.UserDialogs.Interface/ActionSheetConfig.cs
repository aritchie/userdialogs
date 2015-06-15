using System;
using System.Collections.Generic;


namespace Acr.UserDialogs {

    public class ActionSheetConfig {
        public static string DefaultCancelText { get; set; }
        public static string DefaultDestructiveText { get; set; }


        static ActionSheetConfig() {
            DefaultCancelText = "Cancel";
            DefaultDestructiveText = "Remove";
        }


        public string Title { get; set; }
		public ActionSheetOption Cancel { get; set; }
		public ActionSheetOption Destructive { get; set; }
		public IList<ActionSheetOption> Options { get; set; }


        public ActionSheetConfig() {
            this.Options = new List<ActionSheetOption>();
        }


        public ActionSheetConfig SetTitle(string title) {
            this.Title = title;
            return this;
        }


		public ActionSheetConfig SetCancel(string text = null, Action action = null) {
			this.Cancel = new ActionSheetOption(text ?? DefaultCancelText, action);
			return this;
		}


		public ActionSheetConfig SetDestructive(string text = null, Action action = null) {
			this.Destructive = new ActionSheetOption(text ?? DefaultDestructiveText, action);
			return this;
		}


        public ActionSheetConfig Add(string text, Action action = null) {
            this.Options.Add(new ActionSheetOption(text, action));
            return this;
        }
    }
}
