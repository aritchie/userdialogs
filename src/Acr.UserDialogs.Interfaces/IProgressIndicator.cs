using System;


namespace Acr.UserDialogs {

	public interface IProgressIndicator : IDisposable {

		string Title { get; set; }
		int PercentComplete { get; set; }
		bool IsDeterministic { get; set; }
		bool IsShowing { get; }
		void Show();
		void Hide();
	}
}

