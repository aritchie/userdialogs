using System;


namespace Acr.UserDialogs {
	public class ProgressDialogConfig : ProgressConfig {
		public string CancelText { get; set; }
		public Action OnCancel { get; set; }


		public ProgressDialogConfig() {
			this.Title = "Loading";
			this.CancelText = "Cancel";
		}
	}
}

