using System;


namespace Acr.UserDialogs {

	public interface IProgressIndicator : IDisposable {

		int PercentComplete { get; set; }
		bool IsShowing { get; }
		void Show();
		void Hide();
	}
}

