using System;


namespace Acr.UserDialogs {
    
    public class ActionSheetOption {

        public string Text { get; set; }
        public Action Action { get; set; }


        public ActionSheetOption(string text, Action action = null) {
            this.Text = text;
            this.Action = (action ?? (() => {}));
        }


		public void TryExecute() {
			if (this.Action != null)
				this.Action();
		}
    }
}
